using System;
using System.Linq;
using System.Reflection;
using BookmakerBoard.Logics.Ext;
using Microsoft.AspNetCore.Identity;

namespace BookmakerBoard.Logics.Impl
{
  /// <summary>
  /// 
  /// </summary>
  public class PasswordHasher : IPasswordHasher<IdentityUser>
  {
    /// <summary>
    /// Hash a password
    /// </summary>
    public string HashPassword(IdentityUser user, string password)
    {
      var salt = Assembly.GetExecutingAssembly().GetName().Name;
      var saltName = salt + user.UserName;
      var saltPass = password + salt;

      var maxLen = Math.Max(saltName.Length, saltPass.Length);

      var normalName = saltName.Expand(maxLen);
      var normalPass = saltPass.Expand(maxLen);
      var normalSalt = salt.Expand(maxLen);

      return normalName.OR(normalPass).OR(normalSalt);
    }

    /// <summary>
    /// Verify that a password matches the hashedPassword
    /// </summary>
    public PasswordVerificationResult VerifyHashedPassword(IdentityUser user, string hashedPassword, string providedPassword)
    {
      if (hashedPassword == HashPassword(user, providedPassword))
      {
        return PasswordVerificationResult.Success;
      }
      return PasswordVerificationResult.Failed;
    }
  }
}
