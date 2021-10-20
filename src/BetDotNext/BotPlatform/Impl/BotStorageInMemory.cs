using System.Collections.Concurrent;

namespace BetDotNext.BotPlatform.Impl
{
  public class BotStorageInMemory : IBotStorage
  {
    private readonly ConcurrentDictionary<long, BotContext> _botContexts = new ConcurrentDictionary<long, BotContext>();

    public void SaveOrUpdateContext(BotContext context)
    {
      _botContexts.AddOrUpdate(context.Chat.Id, context, (key, val) => context);
    }

    public void DeleteContextByChatId(long chatId)
    {
      _botContexts.TryRemove(chatId, out _);
    }

    public BotContext GetBotContextByChatId(long chatId)
    {
      return _botContexts.TryGetValue(chatId, out var val) ? val : null;
    }
  }
}
