using CelebrationBoard.Domain.Authentication;
using Microsoft.Extensions.Configuration;

namespace CelebrationBoard.Application.Celebrations;

public sealed class LoginCommand : IRequest<Result<string, Error>>
{
  public string Username { get; }
  public string Password { get; }

  public LoginCommand(string username, string password)
  {
    this.Username = username;
    this.Password = password;
  }

  internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<string, Error>>
  {
    private readonly CelebrationBoardContext _context;
    private readonly IIdentityService _identityService;
    private readonly IConfiguration _configuration;

    public LoginCommandHandler(CelebrationBoardContext context, IIdentityService identityService, IConfiguration configuration)
    {
      _context = context;
      this._identityService = identityService;
      this._configuration = configuration;
    }

    public async Task<Result<string, Error>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
      if (await _identityService.CheckPasswordAsync(request.Username, request.Password))
      {
        var token = await _identityService.GenerateTokenAsync(request.Username, _configuration["JWT:Secret"], _configuration["JWT:ValidIssuer"], _configuration["JWT:ValidAudience"]);
        return token;
      }

      return Result.Failure<string, Error>(Errors.User.InvalidCredentials());
    }
  }
}

