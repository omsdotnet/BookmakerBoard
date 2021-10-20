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
      var koefficient = 2;

      if (Number >= 2 && Number <= 10)
      {
        koefficient = 5;
      }
      else if (Number == 11)
      {
        koefficient = 4;
      }
      else if (Number == 1 || Number == 12)
      {
        koefficient = 3;
      }

      foreach (var item in Rates)
      {
        item.Bidder.CurrentScore -= item.RateValue;

        if (WinnerTeams.Contains(item.Team))
        {
          item.Bidder.CurrentScore += item.RateValue * koefficient;
        }
      }
    }
  }
}
