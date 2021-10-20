using System.Threading.Tasks;
using BetDotNext.BotPlatform;
using BetDotNext.BotPlatform.Impl;
using BetDotNext.ExternalServices;
using BetDotNext.Models;
using BetDotNext.Services;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace BetDotNext.Activity
{
  public class HelpActivity : BotActivityBase
  {
    private readonly QueueMessagesService _queueMessagesService;
    private readonly BetPlatformService _betPlatformService;
    private readonly ILogger<HelpActivity> _logger;

    public HelpActivity(IBotStorage botStorage, IBotMediator mediator,
      QueueMessagesService queueMessagesService, BetPlatformService betPlatformService,
      ILogger<HelpActivity> logger) : base(botStorage, mediator)
    {
      _queueMessagesService = queueMessagesService;
      _betPlatformService = betPlatformService;
      _logger = logger;
    }

    public override BotActivityBase SelectActivity<T>(Message message, T context) => null;

    public override Task<bool> CurrentExecuteAsync<T>(Message message, T context)
    {

      var tSuccess = string.Format(StringsResource.HelpText);
      var success = new MessageQueue { Chat = message.Chat, Text = tSuccess };
      _queueMessagesService.Enqueue(success);
      _logger.LogInformation("Response for help in:", message.Chat.Id);

      return Task.FromResult(true);
    }
  }
}
