using System;
using System.Threading.Tasks;
using BetDotNext.Activity.Utils;
using BetDotNext.BotPlatform;
using BetDotNext.BotPlatform.Impl;
using BetDotNext.ExternalServices;
using BetDotNext.Models;
using BetDotNext.Services;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace BetDotNext.Activity.Bet
{
  public class CreatedBetActivity : BotActivityBase
  {
    private const int SegmentLength = 3;
    private readonly QueueMessagesService _queueMessagesService;
    private readonly BetPlatformService _betPlatformService;
    private readonly ILogger<CreatedBetActivity> _logger;

    public CreatedBetActivity(IBotStorage botStorage, IBotMediator mediator,
      QueueMessagesService queueMessagesService, BetPlatformService betPlatformService,
      ILogger<CreatedBetActivity> logger) : base(botStorage, mediator)
    {
      _queueMessagesService = queueMessagesService;
      _betPlatformService = betPlatformService;
      _logger = logger;
    }

    public override BotActivityBase SelectActivity<T>(Message message, T context) => null;

    public override async Task<bool> CurrentExecuteAsync<T>(Message message, T context)
    {
      var chatId = message.Chat.Id;

      string[] parts = message.Text.Split('-', StringSplitOptions.RemoveEmptyEntries);
      if (parts.Length != SegmentLength)
      {
        var err = new MessageQueue { Chat = message.Chat, Text = StringsResource.BetActivityUnexpectedFormatMessage };
        _queueMessagesService.Enqueue(err);
        _logger.LogWarning("Wrong format message from chat {0}", chatId);

        return false;
      }

      _logger.LogInformation("The correct format message from chat {0}", chatId);

      var lm = new MessageQueue { Chat = message.Chat, Text = StringsResource.LoadingMessage };
      _queueMessagesService.Enqueue(lm);

      var fail = new MessageQueue { Chat = message.Chat, Text = StringsResource.FailCreatedActivityMessage };

      try
      {
        var bidder = message.GetUserNameOrLastFirstName();
        var speaker = parts[0].Trim();
        var rate = parts[1].Trim();
        var ride = parts[2].NormalizeRideValue();

        var bet = new CreateBet {Rate = uint.Parse(rate), Ride = ride, Speaker = speaker, Bidder = bidder};
        var currentScore = await _betPlatformService.CreateBetAsync(bet);
        if (!currentScore.HasValue)
        {
          _logger.LogInformation("Fail created bet from chat {0}", chatId);
          _queueMessagesService.Enqueue(fail);
          return false;
        }

        var tSuccess = string.Format(StringsResource.SuccessBetActivity, currentScore);
        var success = new MessageQueue {Chat = message.Chat, Text = tSuccess};
        _queueMessagesService.Enqueue(success);
        _logger.LogInformation("Bet successfully created from chat {0}", chatId);

        return true;
      }
      catch (Exception ex) when (ex is UnexpectedFormatMessageException)
      {
        _logger.LogError(ex, "Error when create bets");

        var formatMessage = new MessageQueue { Chat = message.Chat, Text = ex.Message };
        _queueMessagesService.Enqueue(formatMessage);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error when create bets");
        _queueMessagesService.Enqueue(fail);
      }

      return false;
    }
  }
}
