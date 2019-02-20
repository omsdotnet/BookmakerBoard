using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookmakerBoard.Entities
{
  public class Ride
  {
    public uint Id { get; set; }

    public uint Number { get; set; }

    public List<uint> WinnerTeams { get; set; }

    public List<Rate> Rates { get; set; }
  }
}
