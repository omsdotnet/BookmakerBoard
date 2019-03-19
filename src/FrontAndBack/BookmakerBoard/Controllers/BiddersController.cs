using System.Collections.Generic;
using System.Linq;
using BookmakerBoard.Logics;
using BookmakerBoard.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookmakerBoard.Controllers
{
  [Route("api/[controller]")]
  public class BiddersController : Controller
  {
    private readonly IGame gameEngine;
    private readonly IGameStorage gameStorage;

    public BiddersController(
      IGame game,
      IGameStorage gameStorage)
    {
      this.gameEngine = game;
      this.gameStorage = gameStorage;
    }

    [HttpGet("[action]")]
    public IEnumerable<Bidder> GetAll()
    {
      return gameEngine.Bidders;
    }

    [HttpGet("[action]")]
    public IEnumerable<Bidder> GetTopThree()
    {
      return gameEngine.Bidders
        .OrderByDescending(x => x.CurrentScore)
        .Take(3);
    }


    [HttpPut("{id}")]
    public IActionResult PutBidder([FromRoute] uint id, [FromBody] Bidder item)
    {
      var element = gameEngine.Bidders.SingleOrDefault(x => x.Id == id);

      if(element == null)
      {
        return NotFound();
      }

      element.Name = item.Name;
      element.StartScore = item.StartScore;
      gameEngine.CalculateBiddersCurrentScore();
      gameStorage.Save();

      return Ok();
    }

    [HttpPost]
    public IActionResult PostBidder([FromBody] Bidder item)
    {
      gameEngine.Bidders.Add(item);
      gameEngine.CalculateBiddersCurrentScore();
      gameStorage.Save();

      return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBidder([FromRoute] uint id)
    {
      var element = gameEngine.Bidders.SingleOrDefault(x => x.Id == id);

      if (element == null)
      {
        return NotFound();
      }

      gameEngine.Bidders.Remove(element);

      gameEngine.CalculateBiddersCurrentScore();
      gameStorage.Save();

      return Ok();
    }
  }
}
