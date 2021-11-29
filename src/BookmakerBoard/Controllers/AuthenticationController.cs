using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookmakerBoard.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class AuthenticationController : ControllerBase
  {
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="signInManager"></param>
    public AuthenticationController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
    {
      this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
      this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="login"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    [HttpPost("[action]")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string login, string password)
    {
      var user = await userManager.FindByNameAsync(login);
      var result = await signInManager.CheckPasswordSignInAsync(user, password, false);

      if (result.Succeeded)
      {
        var userPrincipal = await signInManager.CreateUserPrincipalAsync(user);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal,
          new AuthenticationProperties
          {
            ExpiresUtc = DateTime.UtcNow.AddMinutes(200),
            IsPersistent = false,
            AllowRefresh = false
          });

        return Ok();
      }
      else
      {
        return BadRequest("Invalid login/password attempt.");
      }
    }

    /// <summary>
    /// Gets a value that indicates whether the user has been authenticated.
    /// </summary>
    /// <returns>
    /// true if the user was authenticated; otherwise, false.
    /// </returns>
    [HttpGet("[action]")]
    [AllowAnonymous]
    public Task<bool> IsSignIn()
    {
      return Task.FromResult(HttpContext.User.Identity.IsAuthenticated);
    }


    /// <summary>
    /// Sign out the specified authentication scheme.
    /// </summary>
    /// <returns>A task.</returns>
    [HttpGet("[action]")]
    public Task Logout()
    {
      return HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme,
        new AuthenticationProperties
        {
          ExpiresUtc = DateTime.UtcNow.AddMinutes(200),
          IsPersistent = false,
          AllowRefresh = false
        });
    }
  }
}
