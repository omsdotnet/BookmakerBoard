using System;
using System.Linq;
using System.Reflection;
using BookmakerBoard.Logics.Ext;

namespace BookmakerBoard.Logics.Impl
{
  public class Scrambler : IScrambler
  {
    public string GetHash(string name, string pass)
    {
      var salt = Assembly.GetExecutingAssembly().GetName().Name;
      var saltName = salt + name;
      var saltPass = pass + salt;

      var maxLen = Math.Max(saltName.Length, saltPass.Length);

      var normalName = saltName.Expand(maxLen);
      var normalPass = saltPass.Expand(maxLen);
      var normalSalt = salt.Expand(maxLen);

      return normalName.OR(normalPass).OR(normalSalt);
    }
    
  }
}
