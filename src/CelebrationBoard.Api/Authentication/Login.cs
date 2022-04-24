// namespace CelebrationBoard.Api.Authentication;

// using System.Security.Claims;
// using CelebrationBoard.Infrastructure.Identity;
// using Microsoft.AspNetCore.Identity;


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
//     var user = await userManager.FindByNameAsync(request.Username);
//     if (user is not null && await userManager.CheckPasswordAsync(user, request.Password))
//     {
//       var userRoles = await userManager.GetRolesAsync(user);

//       var authClaims = new List<Claim>
//                 {
//                     new Claim(ClaimTypes.Name, user.UserName),
//                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                 };

//       foreach (var userRole in userRoles)
//       {
//         authClaims.Add(new Claim(ClaimTypes.Role, userRole));
//       }

//       var token = GetToken(authClaims);

//       return Ok(new
//       {
//         token = new JwtSecurityTokenHandler().WriteToken(token),
//         expiration = token.ValidTo
//       });
//     }
//     return Unauthorized(new ProblemDetails()
//     {
//       Title = "Unauthorised access",
//       Detail = "You do not have the permissions to access this resource. Please make sure you are using the correct credentials and are accessing the correct resource."
//     });

//     JwtSecurityToken GetToken(List<Claim> authClaims)
//     {
//       var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

//       var token = new JwtSecurityToken(
//           issuer: configuration["JWT:ValidIssuer"],
//           audience: configuration["JWT:ValidAudience"],
//           expires: DateTime.Now.AddHours(3000),
//           claims: authClaims,
//           signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
//           );

//       return token;
//     }
//   }
// }