using Telegram.Bot.Types;

namespace BetDotNext.Activity.Utils
{
  public static class MessageExtensions
  {
    public static string GetUserNameOrLastFirstName(this Message message)
    {
      return !string.IsNullOrEmpty(message.Chat.Username)
        ? message.Chat.Username
        : $"{message.Chat.LastName} {message.Chat.FirstName}";
    }

    public static uint NormalizeRideValue(this string rideValue)
    {
      if (string.IsNullOrEmpty(rideValue))
      {
        throw new UnexpectedFormatMessageException(StringsResource.NominationNotEmpty);
      }

      var rideNumber = rideValue
        .Trim()
        .ToLower()
        .Replace("11", "-")
        .Replace("12", "-")
        .Replace("13", "-")
        .Replace("top3", "11")
        .Replace("top5", "12")
        .Replace("top10", "13");

      return uint.TryParse(rideNumber, out var ride) ? 
        ride : 
        throw new UnexpectedFormatMessageException(StringsResource.IncorectNomination);
    }
  }
}
