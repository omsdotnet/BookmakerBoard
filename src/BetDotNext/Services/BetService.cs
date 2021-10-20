using System;
using System.Collections.Generic;
using BetDotNext.BotPlatform;
using BetDotNext.Utils;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;

namespace BetDotNext.Services
{
  public class BetService
  {
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly ILogger<BetService> _logger;
    private readonly IBot _bot;

    public BetService(ITelegramBotClient telegramBotClient,
      ILogger<BetService> logger, IBot bot)
    {
      Ensure.NotNull(telegramBotClient, nameof(telegramBotClient));
      Ensure.NotNull(logger, nameof(logger));
      Ensure.NotNull(bot, nameof(bot));

      _telegramBotClient = telegramBotClient;
      _logger = logger;
      _bot = bot;
    }

    public void Start()
    {
      try
      {
        _logger.LogInformation("Bot Started");
        if (_telegramBotClient.IsReceiving)
        {
          return;
        }

        _telegramBotClient.OnMessage += OnMessageReceive;
        _telegramBotClient.OnInlineQuery += TelegramBotClientOnOnInlineQuery;
        _telegramBotClient.StartReceiving();
      }
      catch (ApiRequestException ex)
      {
        _logger.LogCritical($"Message: {ex.Message}");
        throw;
      }
    }

    private async void TelegramBotClientOnOnInlineQuery(object sender, InlineQueryEventArgs e)
    {
      string url = "https://images.ctfassets.net/9n3x4rtjlya6/2JnpX9q4fypdaeNjdhhbzz/dc2d2542b6fd121ed7a1e71d557802b6/prosin.jpg";
      var l = new List<InlineQueryResultPhoto>();
      l.Add(new InlineQueryResultPhoto("1", url + "?w=200", url + "?w=150")
      {
        Caption = "Рома Просин",
        Description = "Спикер из Райфа",
        Title = "Code Review",
      });

      await _telegramBotClient.AnswerInlineQueryAsync(e.InlineQuery.Id, l, isPersonal: true, cacheTime: 0);
    }

    public void Stop()
    {
      try
      {
        _logger.LogInformation("Bot Stopped");
        _telegramBotClient.OnMessage -= OnMessageReceive;
        _telegramBotClient.StopReceiving();
      }
      catch (Exception ex)
      {
        _logger.LogCritical($"Message: {ex.Message}");
        throw;
      }
    }

    private void OnMessageReceive(object sender, MessageEventArgs args)
    {
      Message message = args.Message;
      if (message?.Chat == null)
      {
        return;
      }

      MessageHandler(message);
    }

    private async void MessageHandler(Message message)
    {
      try
      {
        string user = !string.IsNullOrEmpty(message.Chat.Username) ?
          message.Chat.Username :
          $"{message.Chat.LastName} {message.Chat.FirstName}";

        _logger.LogInformation("Received message {0} from chat {1} user {2}. ", 
          message.Text, 
          message.Chat.Id, 
          user);

        await _bot.StartDialogAsync(message);
      }
      catch (Exception ex)
      {
        _logger.LogCritical(ex, "The message cannot be processed.");
      }
    }
  }
}