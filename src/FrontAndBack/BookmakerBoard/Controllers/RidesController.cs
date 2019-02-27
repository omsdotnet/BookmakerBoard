using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookmakerBoard.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookmakerBoard.Controllers
{
  [Route("api/[controller]")]
  public class RidesController : Controller
  {

    [HttpGet("[action]")]
    public IEnumerable<Ride> GetAll()
    {
      var rng = new Random();
      return Enumerable.Range(0, 4).Select(index => new Ride
      {
        Id = (uint)index,
        Number = (uint)index + 1,
        Rates = new List<Rate>
        {
          new Rate { Id = 0, Bidder = new Bidder() { Id = 0 }, Team = 0, RateValue = 100 },
          new Rate { Id = 1, Bidder = new Bidder() { Id = 1 }, Team = 1, RateValue = 100 },
          new Rate { Id = 2, Bidder = new Bidder() { Id = 2 }, Team = 2, RateValue = 100 },
          new Rate { Id = 3, Bidder = new Bidder() { Id = 3 }, Team = 3, RateValue = 100 },
        },
        WinnerTeams = new List<uint>
        {
          (uint)rng.Next(5),
          (uint)rng.Next(5)
        }
      }).OrderByDescending(x => x.Number);
    }

  }
}
