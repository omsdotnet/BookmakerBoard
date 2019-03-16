using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookmakerBoard.Models;

namespace BookmakerBoard.Logics.Impl
{
  public class GameStorage : IGameStorage
  {
    private readonly IGame gameEngine;

    public GameStorage(IGame game)
    {
      this.gameEngine = game;
    }



    public void Load()
    {
      var rng = new Random();

      string[] NamesTeams = new[]
      {
         "Злой рок", "Бригада", "Слишком умная команда", "Команда А"
      };

      gameEngine.Teams = Enumerable.Range(0, NamesTeams.Length).Select(index => new Team
      {
        Id = (uint)index,
        Name = NamesTeams[index]
      }).ToList();


      string[] NamesBidders = new[]
      {
         "Иван", "Петр", "Том Хэнкс", "Чача", "Лютый", "Брюс", "Ара", "Гиви"
      };

      gameEngine.Bidders = Enumerable.Range(0, NamesBidders.Length).Select(index => new Bidder
      {
        Id = (uint)index,
        Name = NamesBidders[index],
        StartScore = 1000,
        CurrentScore = (uint)rng.Next(0, 1000)
      }).ToList();

      gameEngine.Rides = Enumerable.Range(0, 4).Select(index => new Ride
      {
        Id = (uint)index,
        Number = (uint)index + 1,
        Rates = new List<Rate>
        {
          new Rate { Id = 0, Bidder = new Bidder() { Id = 0 }, Team = 0, RateValue = 100 },
          new Rate { Id = 1, Bidder = new Bidder() { Id = 1 }, Team = 1, RateValue = 100 },
          new Rate { Id = 2, Bidder = new Bidder() { Id = 2 }, Team = 2, RateValue = 100 },
          new Rate { Id = 3, Bidder = new Bidder() { Id = 3 }, Team = 3, RateValue = 100 },
        },
        WinnerTeams = new List<uint>
        {
          (uint)rng.Next(5),
          (uint)rng.Next(5)
        }
      }).OrderByDescending(x => x.Number).ToList();

      gameEngine.CalculateBiddersCurrentScore();
    }

    public void Save()
    {

    }
  }
}
