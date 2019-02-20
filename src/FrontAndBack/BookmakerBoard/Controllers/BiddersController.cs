using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookmakerBoard.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookmakerBoard.Controllers
{
  [Route("api/[controller]")]
  public class BiddersController : Controller
  {
    private static string[] Names = new[]
    {
            "Иван", "Петр", "Том Хэнкс", "Чача", "Лютый", "Брюс", "Ара", "Гиви"
    };

    [HttpGet("[action]")]
    public IEnumerable<Bidder> GetAll()
    {
      var rng = new Random();
      return Enumerable.Range(0, Names.Length).Select(index => new Bidder
      {
        Id = (uint)index,
        Name = Names[index],
        StartScore = 1000,
        CurrentScore = (uint)rng.Next(0, 1000)
      });
    }
  }
}
