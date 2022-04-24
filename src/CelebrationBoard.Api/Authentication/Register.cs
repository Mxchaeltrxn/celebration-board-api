namespace CelebrationBoard.Api.Authentication;

using System.Security.Claims;
using CelebrationBoard.Domain.Authentication;
using CelebrationBoard.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;


public class RegisterController : BaseController
{
  [ApiVersion("1.0")]
  [HttpPost]
  [SwaggerOperation(
        Summary = "Registers a new user.",
        Description = "Creates a user that can then log in.",
        Tags = new[] { "AuthenticateEndpoints" })]
  [SwaggerResponse(200, "Successfully logged in and retrieved bearer token.")]
  [SwaggerResponse(400, "Invalid credentials provided in the payload.")]
  [SwaggerResponse(409, "There is already a user that has either the username or email provided in the payload.")]
  [SwaggerResponse(500, "Unexpected server error.")]
  public async Task<IActionResult> Register(RegisterRequest request)
  {
    {
      //   var duplicateUsername = (await this.userManager.FindByNameAsync(request.Username)) is not null;
      //   var duplicateEmailAddress = (await this.userManager.FindByEmailAsync(request.EmailAddress)) is not null;
      //   if (duplicateUsername && duplicateEmailAddress)
      //   {
      //     return this.Conflict(new ConflictProblemDetails(
      //       title: "Conflicting username and email",
      //       detail: "A user with the provided username and email address already exists (though it may not be the same user with these details). Please choose a different username and email.",
      //       conflictingProperties: new Dictionary<string, dynamic>() {
      //           { "username", request.Username },
      //           { "emailAddress", request.EmailAddress }
      //     }));
      //   }
      //   else if (duplicateUsername)
      //   {
      //     return this.Conflict(new ConflictProblemDetails(
      //         title: "Conflicting username",
      //       detail: "A user with provided username already exists. Please choose a different username.",
      //       conflictingProperties: new Dictionary<string, dynamic>() {
      //           { "username", request.Username }
      //     }));
      //   }
      //   else if (duplicateEmailAddress)
      //   {
      //     return this.Conflict(new ConflictProblemDetails(
      //         title: "Conflicting email address",
      //       detail: "A user with provided email address already exists. Please choose a different email address.",
      //       conflictingProperties: new Dictionary<string, dynamic>() {
      //           { "emailAddress", request.EmailAddress }
      //     }));
      //   }
      // }

      // var result = await this.userManager.CreateUserAsync(request.Username, request.EmailAddress, request.Password);
      // var user = new User(Guid.Parse(result.Value));
      // if (result.IsFailure)
      // {
      //   return this.BadRequest(new ProblemDetails()
      //   {
      //     Title = "Invalid registration credentials",
      //     Detail = "Username, email or password is not passing validation."
      //   });
      // }

      var sendOrError = await base.Mediator.Send(new RegisterCommand(
   username: request.Username,
   emailAddress: request.EmailAddress,
   password: request.Password
 ));

      if (sendOrError.IsFailure)
        return this.FromResult(sendOrError);

      return this.Ok(sendOrError.Value);
    }
  }
}