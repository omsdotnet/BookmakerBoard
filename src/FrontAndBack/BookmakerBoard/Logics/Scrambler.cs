using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BookmakerBoard.Logics
{
  public static class Scrambler
  {
    public static string GetHash(string name, string pass)
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

    public static string OR(this string key, string input)
    {
      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < input.Length; i++)
        sb.Append((char)(input[i] | key[(i % key.Length)]));
      String result = sb.ToString();

      return result;
    }

    public static string Expand(this string str, int length)
    {
      if (length <= str.Length) return str.Substring(0, length);
      while (str.Length * 2 <= length)
      {
        str += str;
      }
      if (str.Length < length)
      {
        str += str.Substring(0, length - str.Length);
      }
      return str;
    }
  }
}
