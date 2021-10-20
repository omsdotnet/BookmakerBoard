using System;
using System.Threading.Tasks;
using BetDotNext.BotPlatform;
using BetDotNext.BotPlatform.Impl;
using BetDotNext.Models;
using BetDotNext.Services;
using Telegram.Bot.Types;

namespace BetDotNext.Activity.Bet
{
  public class BetActivity : BotActivityBase
  {
    private readonly QueueMessagesService _queueMessagesService;

    public BetActivity(IBotStorage botStorage, IBotMediator mediator, 
      QueueMessagesService queueMessagesService) : base(botStorage, mediator)
    {
      _queueMessagesService = queueMessagesService;
    }

    public override BotActivityBase SelectActivity<T>(Message message, T context)
    {
      return GetActivity<CreatedBetActivity>();
    }

    public override Task<bool> CurrentExecuteAsync<T>(Message message, T context)
    {
      var newMessage = new MessageQueue { Chat = message.Chat, StartTime = DateTime.UtcNow, Text = StringsResource.AcceptedBetActivityMessage };
      _queueMessagesService.Enqueue(newMessage);

      return Task.FromResult(true);
    }
  }
}
