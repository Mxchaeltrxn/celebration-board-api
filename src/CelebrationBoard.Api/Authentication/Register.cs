namespace CelebrationBoard.Api.Authentication;

public class RegisterController : BaseController
{
  [AllowAnonymous]
  [ApiVersion("1.0")]
  [HttpPost("register")]
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