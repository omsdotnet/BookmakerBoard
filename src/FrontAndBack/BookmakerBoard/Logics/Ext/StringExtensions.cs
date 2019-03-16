using System.Text;

namespace BookmakerBoard.Logics.Ext
{
  public static class StringExtensions
  {
    public static string OR(this string key, string input)
    {
      var sb = new StringBuilder();

      for (int i = 0; i < input.Length; i++)
      {
        sb.Append((char)(input[i] | key[(i % key.Length)]));
      } 

      return sb.ToString();
    }

    public static string Expand(this string str, int length)
    {
      if (length <= str.Length)
      {
        return str.Substring(0, length);
      }

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
