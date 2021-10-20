using System;

namespace BetDotNext.Utils
{
    public static class Ensure
    {
        public static void NotNull<T>(T argument, string argumentName) where T : class {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}