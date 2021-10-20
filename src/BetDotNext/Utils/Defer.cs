using System;

namespace BetDotNext.Utils
{
  public class Defer : IDisposable
  {
    private readonly Action _action;

    public Defer(Action action)
    {
      Ensure.NotNull(action, nameof(action));
      _action = action;
    }

    public void Dispose()
    {
      _action.Invoke();
    }
  }
}
