using BetDotNext.Models;
using BetDotNext.Utils;
using MongoDB.Driver;

namespace BetDotNext.Services
{
  public class ConversationService
  {
    private readonly IMongoCollection<Conversation> _conversation;

    public ConversationService(IMongoDatabase mongoDatabase)
    {
      Ensure.NotNull(mongoDatabase, nameof(mongoDatabase));

      _conversation = mongoDatabase.GetCollection<Conversation>(typeof(Conversation).Name);
    }

    public Conversation GetConversationByChatId(long chatId)
    {
      var filterByChatId = Builders<Conversation>.Filter.Eq(p => p.Chat.Id, chatId);
      return _conversation.Find(filterByChatId).FirstOrDefault();
    }

    public void CreateAndUpdateConversation(Conversation conversation)
    {
      if (GetConversationByChatId(conversation.Chat.Id) == null)
      {
        _conversation.InsertOne(conversation);
      }
      else
      {
        var filterByChatId = Builders<Conversation>.Filter.Eq(p => p.Chat.Id, conversation.Chat.Id);
        var update = Builders<Conversation>.Update.Set(p => p.Command, conversation.Command);
        _conversation.UpdateOne(filterByChatId, update);
      }
    }

    public void DeleteConversation(long chatId)
    {
      var filterByChatId = Builders<Conversation>.Filter.Eq(p => p.Chat.Id, chatId);
      _conversation.DeleteOne(filterByChatId);
    }
  }
}
