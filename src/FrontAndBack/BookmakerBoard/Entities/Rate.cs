using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookmakerBoard.Entities
{
  public class Rate
  {
    public uint Id { get; set; }

    public uint Bigger { get; set; }

    public uint Team { get; set; }

    public uint RateValue { get; set; }
  }
}
