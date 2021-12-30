using System.Collections.Generic;
using System.Linq;
using BookmakerBoard.Models;
using BookmakerBoard.Logics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BookmakerBoard.Controllers
{
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class TeamsController : ControllerBase
  {
    private readonly IGame gameEngine;
    private readonly IGameStorage gameStorage;

    public TeamsController(
      IGame game,
      IGameStorage gameStorage)
    {
      this.gameEngine = game;
      this.gameStorage = gameStorage;
    }

    [HttpGet("[action]")]
    [AllowAnonymous]
    public IEnumerable<Team> GetAll()
    {
      return gameEngine.Teams;
      
    }

    [HttpPut("{id}")]
    [AllowAnonymous]
    public IActionResult PutTeam([FromRoute] uint id, [FromBody] Team item)
    {
      var element = gameEngine.Teams.SingleOrDefault(x => x.Id == id);

      if (element == null)
      {
        return NotFound();
      }

      element.Name = item.Name;
      gameStorage.Save();

      return Ok();
    }

    [HttpPost]
    public IActionResult PostTeam([FromBody] Team item)
    {
      gameEngine.Teams.Add(item);
      gameStorage.Save();

      return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTeam([FromRoute] uint id)
    {
      var element = gameEngine.Teams.SingleOrDefault(x => x.Id == id);

      if (element == null)
      {
        return NotFound();
      }

      gameEngine.Teams.Remove(element);

      gameEngine.CalculateBiddersCurrentScore();
      gameStorage.Save();

      return Ok();
    }

  }
}
