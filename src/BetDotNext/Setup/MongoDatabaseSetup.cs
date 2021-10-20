using System;
using BetDotNext.Models;
using MongoDB.Driver;

namespace BetDotNext.Setup
{
    public static class MongoDatabaseSetup
    {
        public static IMongoDatabase MongoDbInit(this IMongoDatabase mongoDatabase)
        {
            var queue = mongoDatabase.GetCollection<MessageQueue>(typeof(MessageQueue).Name);
            var users = mongoDatabase.GetCollection<User>(typeof(User).Name);
            var conversation = mongoDatabase.GetCollection<Conversation>(typeof(Conversation).Name);
            
            var optionsUnique = new CreateIndexOptions
            {
                Unique = true,
                Background = true
            };

            var optionsBackground = new CreateIndexOptions
            {
                Background = true,
            };

            try
            {
                var userIndexModel = new CreateIndexModel<User>(
                    Builders<User>.IndexKeys.Ascending(x => x.UserId), optionsUnique);
                
                var queueIdIndexModel = new CreateIndexModel<MessageQueue>(
                    Builders<MessageQueue>.IndexKeys.Ascending(x => x.Chat.Id), optionsBackground);
                
                var queueStartTimeIndex = new CreateIndexModel<MessageQueue>(
                    Builders<MessageQueue>.IndexKeys.Ascending(x => x.StartTime), optionsBackground);

                var conversationIndex = new CreateIndexModel<Conversation>(
                  Builders<Conversation>.IndexKeys.Ascending(x => x.Chat.Id), optionsUnique);
                
                users.Indexes.CreateOne(userIndexModel);
                queue.Indexes.CreateMany(new[] { queueIdIndexModel, queueStartTimeIndex });
                conversation.Indexes.CreateOne(conversationIndex);
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message, ex);
            }

            return mongoDatabase;
        }
    }
}