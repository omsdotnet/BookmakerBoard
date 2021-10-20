using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace BetDotNext.BotPlatform
{
  public interface IBot
  {
    IBot AddActivity(string command, Type type);
    Task StartDialogAsync(Message message, CancellationToken token = default);
  }
}
