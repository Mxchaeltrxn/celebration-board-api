namespace CelebrationBoard.Application.Celebrations;

public sealed class DeleteCelebrationCommand : IRequest<UnitResult<Error>>
{
  public readonly long Id;
  public DeleteCelebrationCommand(long id)
  {
    Id = id;
  }

  internal sealed class DeleteCelebrationCommandHandler : IRequestHandler<DeleteCelebrationCommand, UnitResult<Error>>
  {
    private readonly CelebrationBoardContext _context;

    public DeleteCelebrationCommandHandler(CelebrationBoardContext context)
    {
      _context = context;
    }

    public async Task<UnitResult<Error>> Handle(DeleteCelebrationCommand request, CancellationToken cancellationToken)
    {
      var celebration = this._context.Set<Celebration>().Find(request.Id);
      if (celebration is null)
        return UnitResult.Failure(Errors.General.NotFound(nameof(Celebration), request.Id));

      this._context.Set<Celebration>().Remove(celebration!);
      await this._context.SaveChangesAsync(cancellationToken);

      return UnitResult.Success<Error>();
    }
  }
}

