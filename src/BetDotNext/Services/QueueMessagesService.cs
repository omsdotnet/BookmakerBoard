using System;
using System.Collections.Generic;
using BetDotNext.Models;
using BetDotNext.Utils;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BetDotNext.Services
{
  public class QueueMessagesService
  {
    private readonly IMongoCollection<MessageQueue> _messageQueue;

    public QueueMessagesService(IMongoDatabase mongoDatabase)
    {
      Ensure.NotNull(mongoDatabase, nameof(mongoDatabase));

      _messageQueue = mongoDatabase.GetCollection<MessageQueue>(typeof(MessageQueue).Name);
    }

    public void Enqueue(MessageQueue messageQueue)
    {
      _messageQueue.InsertOne(messageQueue);
    }

    public void Dequeue(ObjectId id)
    {
      _messageQueue.FindOneAndDelete(new BsonDocument("_id", id));
    }

    public IEnumerable<MessageQueue> TopMessages(int limit)
    {
      if (limit <= 0)
      {
        throw new ArgumentOutOfRangeException(nameof(limit));
      }

      var sort = Builders<MessageQueue>.Sort.Ascending(m => m.StartTime);
      var timePriority = Builders<MessageQueue>.Filter.Gt(m => m.StartTime, DateTime.UtcNow.AddMinutes(-5));

      return _messageQueue.Find(timePriority).Limit(limit).Sort(sort).ToEnumerable();
    }
  }
}