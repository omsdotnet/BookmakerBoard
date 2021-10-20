namespace BetDotNext.BotPlatform
{
  public interface IBotMediator
  {
    T GetActivity<T>() where T : class;
  }
}
