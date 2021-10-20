using System;

namespace BetDotNext.BotPlatform.Impl
{
  public class BotMediator : IBotMediator
  {
    private readonly IServiceProvider _serviceProvider;

    public BotMediator(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
    }

    public T GetActivity<T>() where T : class => _serviceProvider.GetService(typeof(T)) as T;
  }
}
