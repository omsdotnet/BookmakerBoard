using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookmakerBoard.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookmakerBoard.Controllers
{
  [Route("api/[controller]")]
  public class AuthenticationController : Controller
  {

    [HttpGet("[action]")]
    public bool ChekLogin(string name, string password)
    {

      return (name == "admin") && (password == "admin");
    }

  }
}
