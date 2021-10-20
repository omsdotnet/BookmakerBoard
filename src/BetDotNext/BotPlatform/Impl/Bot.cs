using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace BetDotNext.BotPlatform.Impl
{
  public class Bot : IBot
  {
    private readonly IServiceProvider _serviceProvider;
    private readonly IBotStorage _botStorage;
    private readonly ILogger<Bot> _logger;

    private readonly Dictionary<string, Type> _commands = new Dictionary<string, Type>();

    public Bot(IServiceProvider serviceProvider, IBotStorage botStorage, ILogger<Bot> logger)
    {
      _serviceProvider = serviceProvider;
      _botStorage = botStorage;
      _logger = logger;
    }

    public IBot AddActivity(string command, Type type)
    {
      _commands.Add(command, type);
      return this;
    }

    public async Task StartDialogAsync(Message message, CancellationToken token = default)
    {
      if (message?.Chat == null)
      {
        throw new ArgumentNullException(nameof(message));
      }

      if (_commands.TryGetValue(message.Text, out var commandType))
      {
        _botStorage.DeleteContextByChatId(message.Chat.Id);
        var command = (BotActivityBase) _serviceProvider.GetService(commandType);
        if (command == null)
        {
          _logger.LogCritical($"Command not fount in dependency injection: {message.Text}");
          return;
        }

        await command.ExecuteConversationAsync(message, (BotContext)null);
        return;
      }

      var context = _botStorage.GetBotContextByChatId(message.Chat.Id);
      if (context != null)
      {
        if (_commands.TryGetValue(context.LastCommand, out var type))
        {
          var command = (BotActivityBase)_serviceProvider.GetService(type);
          if (command == null)
          {
            _logger.LogCritical($"Command not fount in dependency injection: {message.Text}");
            return;
          }

          await command.ExecuteConversationAsync(message, context);
        }
      }
    }
  }
}
