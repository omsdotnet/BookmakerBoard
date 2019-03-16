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
  public class BiddersController : Controller
  {
    private readonly IGame gameEngine;

    public BiddersController(IGame game)
    {
      gameEngine = game;
    }

    [HttpGet("[action]")]
    public IEnumerable<Bidder> GetAll()
    {
      return gameEngine.Bidders;
    }
  }
}
