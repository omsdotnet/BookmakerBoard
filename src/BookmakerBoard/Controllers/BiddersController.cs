using BookmakerBoard.Logics;
using BookmakerBoard.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookmakerBoard.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BiddersController : ControllerBase
    {
        private readonly IGame gameEngine;
        private readonly IGameStorage gameStorage;

        public BiddersController(
          IGame game,
          IGameStorage gameStorage)
        {
            gameEngine = game;
            this.gameStorage = gameStorage;
        }


        [HttpGet("[action]")]
        [AllowAnonymous]
        public IEnumerable<Bidder> GetAll()
        {
            return gameEngine.Bidders;
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public IEnumerable<Bidder> GetTop()
        {
            return gameEngine.Bidders
              .OrderByDescending(x => x.CurrentScore);
        }


        [HttpPut("{id}")]
        public IActionResult PutBidder([FromRoute] uint id, [FromBody] Bidder item)
        {
            var element = gameEngine.Bidders.SingleOrDefault(x => x.Id == id);

            if (element == null)
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
