using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookmakerBoard.Logics;
using BookmakerBoard.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookmakerBoard.Controllers
{
  [Route("api/[controller]")]
  public class RidesController : Controller
  {
    private readonly IGame gameEngine;

    public RidesController(IGame game)
    {
      gameEngine = game;
    }

    [HttpGet("[action]")]
    public IEnumerable<Ride> GetAll()
    {
      return gameEngine.Rides;
    }

  }
}
