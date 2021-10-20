using System.Threading.Tasks;
using BetDotNext.BotPlatform;
using BetDotNext.BotPlatform.Impl;
using BetDotNext.Models;
using BetDotNext.Services;
using Telegram.Bot.Types;

namespace BetDotNext.Activity
{
  public class StartActivity : BotActivityBase
  {
    private readonly QueueMessagesService _queueMessagesService;

    public StartActivity(IBotStorage botStorage, IBotMediator mediator, 
      QueueMessagesService queueMessagesService) : base(botStorage, mediator)
    {
      _queueMessagesService = queueMessagesService;
    }

    public override BotActivityBase SelectActivity<T>(Message message, T context) => null;

    public override Task<bool> CurrentExecuteAsync<T>(Message message, T context)
    {
      var newMessage = new MessageQueue { Chat = message.Chat, Text = StringsResource.StartActivityMessage };
      _queueMessagesService.Enqueue(newMessage);

      return Task.FromResult(false);
    }
  }
}
