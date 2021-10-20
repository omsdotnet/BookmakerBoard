using BetDotNext.BotPlatform.Impl;

namespace BetDotNext.BotPlatform
{
  public interface IBotStorage
  {
    void SaveOrUpdateContext(BotContext context);
    void DeleteContextByChatId(long chatId);
    BotContext GetBotContextByChatId(long chatId);
  }
}
