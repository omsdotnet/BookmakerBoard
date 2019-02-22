using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookmakerBoard.Entities
{
  public class AuthenticationResult
  {
    public bool Authentificated { get; set; }

    public string Key { get; set; }
  }
}
