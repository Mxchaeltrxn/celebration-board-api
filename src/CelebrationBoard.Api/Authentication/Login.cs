namespace CelebrationBoard.Api.Authentication;
using CelebrationBoard.Domain.Authentication;

public class LoginController : BaseController
{
  [AllowAnonymous]
  [ApiVersion("1.0")]
  [HttpPost("login")]
  [SwaggerOperation(
    Summary = "Logs in an existing user.",
    Description = "Returns a bearer token for a created user.",
    Tags = new[] { "AuthenticateEndpoints" })
  ]
  [SwaggerResponse(200, "Successfully logged in and retrieved bearer token.")]
  [SwaggerResponse(400, "Credentials provided in the payload cannot be empty.")]
  [SwaggerResponse(401, "Credentials provided in the payload do not match a user in the database.")]
  [SwaggerResponse(500, "Unexpected server error.")]
  public async Task<IActionResult> Login(LoginRequest request)
  {
    var sendOrError = await base.Mediator.Send(new LoginCommand(
   username: request.Username,
   password: request.Password
 ));

    if (sendOrError.IsFailure)
      return this.FromResult(sendOrError);

    return this.Ok(sendOrError.Value);
  }
}