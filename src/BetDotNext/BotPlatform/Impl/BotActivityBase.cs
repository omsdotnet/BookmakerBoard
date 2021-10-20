using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace BetDotNext.BotPlatform.Impl
{
  public abstract class BotActivityBase
  {
    protected IBotStorage BotStorage { get; }
    private readonly IBotMediator _mediator;

    protected BotActivityBase(IBotStorage botStorage, IBotMediator mediator)
    {
      BotStorage = botStorage;
      _mediator = mediator;
    }

    protected T GetActivity<T>() where T : class => _mediator.GetActivity<T>();

    public virtual async Task ExecuteConversationAsync<T>(Message message, T context) where T : BotContext
    {
      if (context == null)
      {
        var success = await CurrentExecuteAsync(message, (T) null);
        if (success)
        {
          SaveContextActivity(message);
        }
        return;
      }

      var conversation = SelectActivity(message, context.Context);
      if (conversation != null)
      {
        await conversation.ExecuteConversationAsync(message, context.Context);
      }
    }

    public abstract BotActivityBase SelectActivity<T>(Message message, T context) where T : BotContext;

    public abstract Task<bool> CurrentExecuteAsync<T>(Message message, T context) where T : BotContext;

    private void SaveContextActivity(Message message)
    {
      var context = new BotContext { Chat = message.Chat, LastCommand = message.Text };
      var currentContext = BotStorage.GetBotContextByChatId(message.Chat.Id);
      if (currentContext == null)
      {
        BotStorage.SaveOrUpdateContext(context);
      }
      else
      {
        currentContext.Context = context;
        BotStorage.SaveOrUpdateContext(currentContext);
      }
    }
  }
}