using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookmakerBoard.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookmakerBoard.Controllers
{
  [Route("api/[controller]")]
  public class TeamsController : Controller
  {
    private static string[] Names = new[]
    {
            "Злой рок", "Бригада", "Слишком умная команда", "Команда А"
    };

    [HttpGet("[action]")]
    public IEnumerable<Team> GetAll()
    {
      var rng = new Random();
      return Enumerable.Range(0, Names.Length).Select(index => new Team
      {
        Id = (uint)index,
        Name = Names[index]
      });
    }
  }
}
