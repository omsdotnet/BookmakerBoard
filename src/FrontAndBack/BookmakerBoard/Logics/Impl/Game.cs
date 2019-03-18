using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using BookmakerBoard.Models;

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
        item.Calculate();
      }
    }
  }
}
