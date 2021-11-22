using BookmakerBoard.Logics;

namespace BookmakerBoard
{
    public class StoreHostedService : IHostedService
    {
        public StoreHostedService(IGame game, IGameStorage store)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));
            Store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public IGame Game { get; }
        public IGameStorage Store { get; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Store.Load();
            Game.CalculateBiddersCurrentScore();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Store.Save();

            return Task.CompletedTask;
        }
    }
}
