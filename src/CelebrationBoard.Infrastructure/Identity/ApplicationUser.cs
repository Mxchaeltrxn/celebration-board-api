namespace CelebrationBoard.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;


public class ApplicationUser : IdentityUser
{
  public ApplicationUser(string userName, string email)
  {
    this.UserName = userName;
    this.Email = email;

  }
}
