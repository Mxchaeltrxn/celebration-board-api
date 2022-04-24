// namespace CelebrationBoard.Api.Authentication;
// using CelebrationBoard.Domain.Authentication;


// public class LoginController : BaseController
// {
//   // [AllowAnonymous]
//   private readonly IUserManager userManager;
//   private readonly IConfiguration configuration;

//   public LoginController(
//       IUserManager userManager,
//       IConfiguration configuration)
//   {
//     this.userManager = userManager;
//     this.configuration = configuration;
//   }

//   [ApiVersion("1.0")]
//   [HttpPost]
//   [SwaggerOperation(
//     Summary = "Logs in an existing user.",
//     Description = "Returns a bearer token for a created user.",
//     Tags = new[] { "AuthenticateEndpoints" })
//   ]
//   [SwaggerResponse(200, "Successfully logged in and retrieved bearer token.")]
//   [SwaggerResponse(400, "Credentials provided in the payload cannot be empty.")]
//   [SwaggerResponse(401, "Credentials provided in the payload do not match a user in the database.")]
//   [SwaggerResponse(500, "Unexpected server error.")]
//   public async Task<IActionResult> Login(LoginRequest request)
//   {
//     var user = userManager.FindByUserNameAsync(request.Username);
//     if (await userManager.CheckPasswordAsync(user, request.Password))
//     {
//       var token = userManager.GenerateToken(user, configuration["JWT:Secret"], configuration["JWT:ValidIssuer"], configuration["JWT:ValidAudience"]);
//       return Ok(new
//       {
//         token = token,
//         expiration = token.ValidTo
//       });
//     }
//     return Unauthorized(new ProblemDetails()
//     {
//       Title = "Unauthorised access",
//       Detail = "You do not have the permissions to access this resource. Please make sure you are using the correct credentials and are accessing the correct resource."
//     });
//   }
// }