using e_commerce_cs.Models;

using Microsoft.AspNetCore.Identity;

namespace e_commerce_cs.Services
{
  public class HashService
  {
    private readonly PasswordHasher<User> _passwordHasher = new();

    public string HashPassword(User user, string userDTOpassword)
    {
      return _passwordHasher.HashPassword(user, userDTOpassword);
    }
    public bool VerifyPassword(User user, string providedPassword)
    {
      PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(user, user.Password, providedPassword);
      return result == PasswordVerificationResult.Success;
    }
  }
}
