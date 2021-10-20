using System;

namespace BetDotNext.Activity
{
  public class UnexpectedFormatMessageException : Exception
  {
    public UnexpectedFormatMessageException(string message) : base(message)
    {
    }
  }
}
