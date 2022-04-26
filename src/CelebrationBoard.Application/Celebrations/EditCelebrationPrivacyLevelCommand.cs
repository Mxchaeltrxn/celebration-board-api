namespace CelebrationBoard.Application.Celebrations;

public sealed class EditCelebrationPrivacyLevelCommand : IRequest<UnitResult<Error>>
{
  public long UserId { get; }
  public long CelebrationId { get; }
  public string PrivacyLevel { get; }

  public EditCelebrationPrivacyLevelCommand(long userId, long celebrationId, string privacyLevel)
  {
    this.UserId = userId;
    this.CelebrationId = celebrationId;
    this.PrivacyLevel = privacyLevel;
  }


  internal sealed class EditCelebrationPrivacyLevelCommandHandler : IRequestHandler<EditCelebrationPrivacyLevelCommand, UnitResult<Error>>
  {
    private readonly CelebrationBoardContext _context;

    public EditCelebrationPrivacyLevelCommandHandler(CelebrationBoardContext context)
    {
      _context = context;
    }

    public async Task<UnitResult<Error>> Handle(EditCelebrationPrivacyLevelCommand request, CancellationToken cancellationToken)
    {
      var celebration = this._context.Set<Celebration>().Find(request.UserId);
      if (celebration is null)
        return UnitResult.Failure(Errors.General.NotFound(nameof(Celebration), request.UserId));

      celebration!.AccessLevel = Domain.Celebrations.PrivacyLevel.Create(request.PrivacyLevel).Value;
      this._context.Set<Celebration>().Update(celebration);
      await this._context.SaveChangesAsync(cancellationToken);

      return UnitResult.Success<Error>();
    }
  }
}

