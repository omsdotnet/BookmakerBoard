using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookmakerBoard.Models
{
  public class Bidder
  {
    public uint Id { get; set; }

    public string Name { get; set; }

    public uint StartScore { get; set; }

    public long CurrentScore { get; set; }
  }
}
