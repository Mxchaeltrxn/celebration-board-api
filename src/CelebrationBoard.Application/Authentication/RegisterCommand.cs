using CelebrationBoard.Domain.Authentication;

namespace CelebrationBoard.Application.Celebrations;

public sealed class RegisterCommand : IRequest<Result<long, Error>>
{
  public string Username { get; }
  public string EmailAddress { get; }
  public string Password { get; }

  public RegisterCommand(string username, string emailAddress, string password)
  {
    this.Username = username;
    this.EmailAddress = emailAddress;
    this.Password = password;
  }

  internal sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<long, Error>>
  {
    private readonly CelebrationBoardContext _context;
    private readonly IUserManager userManager;

    public RegisterCommandHandler(CelebrationBoardContext context, IUserManager userManager)
    {
      _context = context;
      this.userManager = userManager;
    }

    public async Task<Result<long, Error>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
      var result = await this.userManager.CreateUserAsync(request.Username, request.EmailAddress, request.Password);
      var guid = Guid.Parse(result.Value);
      var user = new User(guid);
      _context.Set<User>().Add(user);
      _context.SaveChanges();
      if (result.IsFailure)
      {
      }

      return Result.Success<long, Error>(user.Id);
    }
  }
}

