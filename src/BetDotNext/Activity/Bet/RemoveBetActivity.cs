using System;
using System.Threading.Tasks;
using BetDotNext.BotPlatform;
using BetDotNext.BotPlatform.Impl;
using BetDotNext.Models;
using BetDotNext.Services;
using Telegram.Bot.Types;

namespace BetDotNext.Activity.Bet
{
  public class RemoveBetActivity : BotActivityBase
  {
    private readonly QueueMessagesService _queueMessagesService;

    public RemoveBetActivity(IBotStorage botStorage, IBotMediator mediator, 
      QueueMessagesService queueMessagesService) : base(botStorage, mediator)
    {
      _queueMessagesService = queueMessagesService;
    }

    public override BotActivityBase SelectActivity<T>(Message message, T context)
    {
      return GetActivity<ConfirmRemoveBetActivity>();
    }

    public override Task<bool> CurrentExecuteAsync<T>(Message message, T context)
    {
      var m = new MessageQueue { Chat = message.Chat, StartTime = DateTime.UtcNow, Text = StringsResource.RemoveBetActivityMessage };
      _queueMessagesService.Enqueue(m);
      return Task.FromResult(true);
    }
  }
}
