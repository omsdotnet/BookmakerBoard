using BookmakerBoard.Models;
using System.Collections.Generic;
using System.Linq;

namespace BookmakerBoard.Logics.Impl
{
  public class Game : IGame
  {
    public List<Bidder> Bidders { get; set; }


    public List<Team> Teams { get; set; }


    public List<Ride> Rides { get; set; }


    public void CalculateBiddersCurrentScore()
    {
      foreach(var item in Bidders)
      {
        item.CurrentScore = item.StartScore;
      }

      foreach (var item in Rides.OrderBy(x => x.Number))
      {
        foreach(var rate in item.Rates)
        {
          rate.Bidder = Bidders.SingleOrDefault(x => x.Id == rate.Bidder.Id);
        }

        item.Rates = item.Rates.Where(x => x.Bidder != null).ToList();

        if(item.WinnerTeams.Any())
        {
          item.Calculate();
        }
      }
    }
  }
}
