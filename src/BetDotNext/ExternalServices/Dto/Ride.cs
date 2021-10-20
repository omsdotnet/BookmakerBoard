using System.Collections.Generic;

namespace BetDotNext.ExternalServices.Dto
{
  public class Ride
  {
    public uint Id { get; set; }
    public uint Number { get; set; }
    public List<uint> WinnerTeams { get; set; }
    public List<Rate> Rates { get; set; }
  }
}
