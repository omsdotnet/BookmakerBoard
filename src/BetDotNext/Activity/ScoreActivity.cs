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
  public class ScoreActivity : BotActivityBase
  {
    private readonly QueueMessagesService _queueMessagesService;
    private readonly ILogger<ScoreActivity> _logger;
    private readonly BetPlatformService _betPlatformService;

    public ScoreActivity(IBotStorage botStorage, IBotMediator mediator,
      QueueMessagesService queueMessagesService, ILogger<ScoreActivity> logger,
      BetPlatformService betPlatformService) : base(botStorage, mediator)
    {
      _queueMessagesService = queueMessagesService;
      _logger = logger;
      _betPlatformService = betPlatformService;
    }

    public override BotActivityBase SelectActivity<T>(Message message, T context) => null;

    public override async Task<bool> CurrentExecuteAsync<T>(Message message, T context)
    {
      try
      {
        var scoreMessage = await _betPlatformService.CurrentScoreAsync(message.GetUserNameOrLastFirstName());
        var newMessage = new MessageQueue {Chat = message.Chat, Text = scoreMessage};
        _queueMessagesService.Enqueue(newMessage);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error when current score");

        var fail = new MessageQueue {Chat = message.Chat, Text = StringsResource.GettingCurrentScoreException};
        _queueMessagesService.Enqueue(fail);
      }

      return false;
    }
  }
}
