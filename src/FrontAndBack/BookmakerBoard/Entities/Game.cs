using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookmakerBoard.Entities
{
  public class Game
  {
    public List<Bidder> Bidders { get; set; }

    public List<Team> Teams { get; set; }

    public List<Ride> Rides { get; set; }


    public void CalculateBiddersCurrentScore()
    {
    }
  }
}
