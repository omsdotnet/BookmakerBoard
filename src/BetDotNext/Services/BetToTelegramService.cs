using System;
using System.Threading;
using System.Threading.Tasks;
using BetDotNext.Utils;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace BetDotNext.Services
{
  internal class BetToTelegramService : IHostedService
  {
    private readonly QueueMessagesService _queueMessagesService;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly ILogger<BetToTelegramService> _logger;

    private Timer _queueTimer;

    public BetToTelegramService(QueueMessagesService queueMessagesService, ITelegramBotClient telegramBotClient,
      ILogger<BetToTelegramService> logger)
    {
      Ensure.NotNull(queueMessagesService, nameof(queueMessagesService));
      Ensure.NotNull(telegramBotClient, nameof(telegramBotClient));
      Ensure.NotNull(logger, nameof(logger));

      _queueMessagesService = queueMessagesService;
      _telegramBotClient = telegramBotClient;
      _logger = logger;
    }

    private async void OnExecuteQueueMessages(object state)
    {
      foreach (var message in _queueMessagesService.TopMessages(30))
      {
        try
        {
          if (message.Chat == null) continue;

          await _telegramBotClient.SendTextMessageAsync(message.Chat, message.Text);
          _queueMessagesService.Dequeue(message.Id);
          _logger.LogInformation("The message {0} from user {1} was deleted", message.Id, message.Chat.Id);
        }
        catch (Exception ex)
        {
          _logger.LogError(ex, "Error when send a message.");
        }
      }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      _queueTimer = new Timer(OnExecuteQueueMessages, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
      return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
      await _queueTimer.DisposeAsync();
    }
  }
}