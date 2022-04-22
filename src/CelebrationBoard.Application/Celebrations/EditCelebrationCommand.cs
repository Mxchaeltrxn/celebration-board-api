namespace CelebrationBoard.Application.Celebrations;

public sealed class EditCelebrationCommand : IRequest<UnitResult<Error>>
{
  public readonly long Id;
  public readonly string? Title;
  public readonly string Content;

  public EditCelebrationCommand(long id, string? title, string content)
  {
    Id = id;
    Title = title;
    Content = content;
  }

  internal sealed class EditCelebrationCommandHandler : IRequestHandler<EditCelebrationCommand, UnitResult<Error>>
  {
    private readonly CelebrationBoardContext _context;

    public EditCelebrationCommandHandler(CelebrationBoardContext context)
    {
      _context = context;
    }

    public async Task<UnitResult<Error>> Handle(EditCelebrationCommand request, CancellationToken cancellationToken)
    {
      var celebration = this._context.Set<Celebration>().Find(request.Id);
      if (celebration is null)
        return UnitResult.Failure(Errors.General.NotFound(nameof(Celebration), request.Id));

      var title = Domain.Celebrations.Title.Create(request.Title).Value;
      var content = Domain.Celebrations.Content.Create(request.Content).Value;
      celebration!.Edit(title: title, content: content);
      this._context.Set<Celebration>().Update(celebration);
      await this._context.SaveChangesAsync(cancellationToken);

      return UnitResult.Success<Error>();
    }
  }
}

