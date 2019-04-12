using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BookmakerBoard.Infrastructure.Identity
{
  public class InMemoryUserStore : IUserPasswordStore<IdentityUser>
  {
    private readonly IdentityUser administrator = new IdentityUser("Азино");

    public InMemoryUserStore()
    {
      administrator.Id = "Administrator";
    }

    public Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(IdentityResult.Failed(new IdentityError()
      {
        Code = $"{nameof(InMemoryUserStore)}_CreateAsync",
        Description = "Создание пользователей не поддерживается."
      }));
    }

    public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(IdentityResult.Failed(new IdentityError()
      {
        Code = $"{nameof(InMemoryUserStore)}DeleteAsync",
        Description = "Удаление пользователей не поддерживается."
      }));
    }

    public void Dispose()
    {      
    }

    public Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
      return Task.FromResult(administrator);
    }

    public Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
      return Task.FromResult(administrator);
    }

    public Task<string> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(administrator.UserName);
    }

    public Task<string> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(@"sѯѿѿѿѡѻgoomsoѷѿѿѿѿso");
    }

    public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(administrator.Id);
    }

    public Task<string> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(administrator.UserName);
    }

    public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(true);
    }

    public Task SetNormalizedUserNameAsync(IdentityUser user, string normalizedName, CancellationToken cancellationToken)
    {
      return Task.CompletedTask;
    }

    public Task SetPasswordHashAsync(IdentityUser user, string passwordHash, CancellationToken cancellationToken)
    {
      return Task.CompletedTask;
    }

    public Task SetUserNameAsync(IdentityUser user, string userName, CancellationToken cancellationToken)
    {
      return Task.CompletedTask;
    }

    public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(IdentityResult.Success);
    }
  }
}
