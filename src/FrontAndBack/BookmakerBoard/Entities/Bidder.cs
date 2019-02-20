using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookmakerBoard.Entities
{
  public class Bidder
  {
    public uint Id { get; set; }

    public string Name { get; set; }

    public uint StartScore { get; set; }

    public ulong CurrentScore { get; set; }
  }
}
