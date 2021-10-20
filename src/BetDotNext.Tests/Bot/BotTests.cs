using System;
using System.Threading.Tasks;
using BetDotNext.BotPlatform;
using BetDotNext.BotPlatform.Impl;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using Telegram.Bot.Types;

namespace BetDotNext.Tests.Bot
{
  public class StartConversation : BotActivityBase
  {
    

    public override BotActivityBase SelectActivity<T>(Message message, T context)
    {
      return GetActivity<YesStartConversation>();
    }

    public override Task<bool> CurrentExecuteAsync<T>(Message message, T context)
    {
      return Task.FromResult(true);
    }

    public StartConversation(IBotStorage botStorage, IBotMediator mediator) : base(botStorage, mediator)
    {
    }
  }

  public class YesStartConversation : BotActivityBase
  {

    public override BotActivityBase SelectActivity<T>(Message message, T context)
    {
      return null;
    }

    public override Task<bool> CurrentExecuteAsync<T>(Message message, T context)
    {
      return Task.FromResult(true);
    }

    public YesStartConversation(IBotStorage botStorage, IBotMediator mediator) : base(botStorage, mediator)
    {
    }
  }

  public class NoStartConversation : BotActivityBase
  {
    public override BotActivityBase SelectActivity<T>(Message message, T context)
    {
      return null;
    }

    public override Task<bool> CurrentExecuteAsync<T>(Message message, T context)
    {
      throw new NotImplementedException();
    }

    public NoStartConversation(IBotStorage botStorage, IBotMediator mediator) : base(botStorage, mediator)
    {
    }
  }

  [TestFixture]
  public class BotTests
  {
    [Test]
    public async Task StartDialogAsyncTest()
    {
      var botStorage = new BotStorageInMemory();

      var serviceProvider = new Mock<IServiceProvider>();
      var botMediator = new Mock<IBotMediator>();

      serviceProvider.Setup(p => p.GetService(It.IsAny<Type>()))
        .Returns(new StartConversation(botStorage, botMediator.Object));


      var bot = new BotPlatform.Impl.Bot(serviceProvider.Object, botStorage, new NullLogger<BotPlatform.Impl.Bot>());
      bot.AddActivity("/start", typeof(StartConversation));


      var message = new Message
      {
        Chat = new Chat
        {
          Id = 15895
        },
        Text = "/start",
      };

      var message1 = new Message
      {
        Chat = new Chat
        {
          Id = 15895
        },
        Text = "kjkjkj",
      };

      await bot.StartDialogAsync(message);
      await bot.StartDialogAsync(message1);
    }
  }
}
