using System.Collections.Generic;
using BookmakerBoard.Models;

namespace BookmakerBoard.Logics
{
  public interface IGame
  {
    List<Bidder> Bidders { get; set; }
    List<Ride> Rides { get; set; }
    List<Team> Teams { get; set; }

    void CalculateBiddersCurrentScore();
  }
}