using System.Collections.Generic;
using System.Linq;
using BookmakerBoard.Logics;
using BookmakerBoard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookmakerBoard.Controllers
{
  [Authorize]
  [Route("api/[controller]")]
  public class RidesController : Controller
  {
    private readonly IGame gameEngine;
    private readonly IGameStorage gameStorage;

    public RidesController(
      IGame game,
      IGameStorage gameStorage)
    {
      this.gameEngine = game;
      this.gameStorage = gameStorage;
    }

    [HttpGet("[action]")]
    [AllowAnonymous]
    public IEnumerable<Ride> GetAll()
    {
      return gameEngine.Rides.OrderBy(x => x.Number);
    }

    [HttpGet("[action]/{id}")]
    [AllowAnonymous]
    public IActionResult GetById(uint id)
    {
      var element = gameEngine.Rides.SingleOrDefault(x => x.Id == id);

      if (element == null)
      {
        return NotFound();
      }

      return Ok(element);
    }



    [HttpPut("{id}")]
    public IActionResult PutRide([FromRoute] uint id, [FromBody] Ride item)
    {
      var element = gameEngine.Rides.SingleOrDefault(x => x.Id == id);

      if (element == null)
      {
        return NotFound();
      }

      element.Number = item.Number;
      element.WinnerTeams = item.WinnerTeams;

      element.Rates = item.Rates;

      gameEngine.CalculateBiddersCurrentScore();
      gameStorage.Save();

      return Ok();
    }

    [HttpPost]
    public IActionResult PostRide([FromBody] Ride item)
    {
      gameEngine.Rides.Add(item);
      gameEngine.CalculateBiddersCurrentScore();
      gameStorage.Save();

      return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteRide([FromRoute] uint id)
    {
      var element = gameEngine.Rides.SingleOrDefault(x => x.Id == id);

      if (element == null)
      {
        return NotFound();
      }

      gameEngine.Rides.Remove(element);

      gameEngine.CalculateBiddersCurrentScore();
      gameStorage.Save();

      return Ok();
    }

  }
}
