using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookmakerBoard.Models;
using BookmakerBoard.Logics;
using Microsoft.AspNetCore.Mvc;

namespace BookmakerBoard.Controllers
{
  [Route("api/[controller]")]
  public class TeamsController : Controller
  {
    private readonly IGame gameLogic;

    public TeamsController(IGame game)
    {
      gameLogic = game;
    }

    [HttpGet("[action]")]
    public IEnumerable<Team> GetAll()
    {
      return gameLogic.Teams;
      
    }
  }
}
