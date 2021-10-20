using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Telegram.Bot.Types;

namespace BetDotNext.Models
{
  public class Conversation
  {
    [BsonId]
    public ObjectId Id { get; set; }
    public Chat Chat { get; set; }
    public string Command { get; set; }
  }
}
