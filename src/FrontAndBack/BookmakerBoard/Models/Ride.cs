using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookmakerBoard.Models
{
  public class Ride
  {
    public uint Id { get; set; }

    public uint Number { get; set; }

    public List<uint> WinnerTeams { get; set; }

    public List<Rate> Rates { get; set; }

    public void Calculate()
    {
      foreach (var item in Rates)
      {
        item.Bidder.CurrentScore -= item.RateValue;

        if (WinnerTeams.Contains(item.Team))
        {
          item.Bidder.CurrentScore += item.RateValue * 2;
        }
      }
    }
  }
}
