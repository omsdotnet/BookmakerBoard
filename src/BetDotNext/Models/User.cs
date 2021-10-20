using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Telegram.Bot.Types;

namespace BetDotNext.Models
{
  public class User
  {
    [BsonId] public ObjectId Id { get; set; }

    public ChatId UserId { get; set; }
    public string UserName { get; set; }
  }
}