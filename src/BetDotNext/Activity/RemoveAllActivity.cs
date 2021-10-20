using System;
using System.Threading.Tasks;
using BetDotNext.Activity.Utils;
using BetDotNext.BotPlatform;
using BetDotNext.BotPlatform.Impl;
using BetDotNext.ExternalServices;
using BetDotNext.Models;
using BetDotNext.Services;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace BetDotNext.Activity
{
  public class RemoveAllActivity : BotActivityBase
  {
    private readonly BetPlatformService _betPlatformService;
    private readonly QueueMessagesService _queueMessagesService;
    private readonly ILogger<RemoveAllActivity> _logger;

    public RemoveAllActivity(IBotStorage botStorage, IBotMediator mediator,
      BetPlatformService betPlatformService, QueueMessagesService queueMessagesService, ILogger<RemoveAllActivity> logger) : base(botStorage, mediator)
    {
      _betPlatformService = betPlatformService;
      _queueMessagesService = queueMessagesService;
      _logger = logger;
    }

    public override BotActivityBase SelectActivity<T>(Message message, T context) => null;

    public override async Task<bool> CurrentExecuteAsync<T>(Message message, T context)
    {
      try
      {
        var res = await _betPlatformService.DeleteAllBetAsync(message.GetUserNameOrLastFirstName());

        var messageSuccess = new MessageQueue {Chat = message.Chat, Text = res};
        _queueMessagesService.Enqueue(messageSuccess);

        return true;
      }
      catch (Exception ex) when (ex is UnexpectedFormatMessageException)
      {
        _logger.LogError(ex, "Error when delete a bet.");

        var formatMessage = new MessageQueue {Chat = message.Chat, Text = ex.Message};
        _queueMessagesService.Enqueue(formatMessage);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error when delete a bet.");

        var fail = new MessageQueue {Chat = message.Chat, Text = StringsResource.FailDeleteActivityMessage};
        _queueMessagesService.Enqueue(fail);
      }

      return false;
    }
  }
}
