using System;
using Microsoft.AspNetCore.Mvc;
using BookmakerBoard.Logics;
using BookmakerBoard.Models;

namespace BookmakerBoard.Controllers
{
  [Route("api/[controller]")]
  public class AuthenticationController : Controller
  {
    private readonly IScrambler scramblerData;

    public AuthenticationController(IScrambler scrambler)
    {
      scramblerData = scrambler;
    }

    [HttpGet("[action]")]
    public AuthenticationResult Login(string name, string password)
    {
      string adminHash = @"soѯѿѿѿѫѵrookeѻѿѿѿѿco";
        
      return new AuthenticationResult()
      {
        Authentificated = scramblerData.GetHash(name, password) == adminHash,

        Key = Guid.NewGuid().ToString()
      };
    }

  }
}
