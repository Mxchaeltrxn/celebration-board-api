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
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(CelebrationBoardContext context, IIdentityService identityService)
    {
      _context = context;
      this._identityService = identityService;
    }

    public async Task<Result<long, Error>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
      var isDuplicateUsername = await _identityService.IsDuplicateUsernameAsync(request.Username);
      var isDuplicateEmailAddress = await _identityService.IsDuplicateEmailAsync(request.EmailAddress);
      if (isDuplicateUsername)
        return Errors.User.UsernameIsTaken();

      if (isDuplicateEmailAddress)
        return Errors.User.EmailIsTaken();

      var createUserOrError = await this._identityService.CreateUserAsync(request.Username, request.EmailAddress, request.Password);
      if (createUserOrError.IsFailure)
        return Errors.User.InvalidCredentials();

      var user = new User(Guid.Parse(createUserOrError.Value));
      _context.Set<User>().Add(user);
      _context.SaveChanges();

      return Result.Success<long, Error>(user.Id);
    }
  }
}

