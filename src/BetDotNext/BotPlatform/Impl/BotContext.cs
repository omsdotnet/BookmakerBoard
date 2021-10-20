using Telegram.Bot.Types;

namespace BetDotNext.BotPlatform.Impl
{
  public class BotContext
  {
    public Chat Chat { get; set; }
    public string LastCommand { get; set; }
    public BotContext Context { get; set; }
  }
}
