using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookmakerBoard.Entities;
using BookmakerBoard.Logics;
using Microsoft.AspNetCore.Mvc;

namespace BookmakerBoard.Controllers
{
  [Route("api/[controller]")]
  public class AuthenticationController : Controller
  {

    [HttpGet("[action]")]
    public AuthenticationResult Login(string name, string password)
    {
      string adminHash = @"soѯѿѿѿѫѵrookeѻѿѿѿѿco";
        
      return new AuthenticationResult()
      {
        Authentificated = Scrambler.GetHash(name, password) == adminHash,

        Key = Guid.NewGuid().ToString()
      };
    }

  }
}
