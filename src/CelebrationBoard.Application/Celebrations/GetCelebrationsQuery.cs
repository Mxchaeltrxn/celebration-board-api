namespace CelebrationBoard.Application.Celebrations;
public sealed class GetCelebrationsQuery : IRequest<Celebration[]>
{
  public GetCelebrationsQuery()
  {
  }

  internal sealed class GetCelebrationsQueryHandler : IRequestHandler<GetCelebrationsQuery, Celebration[]>
  {
    private readonly CelebrationBoardContext _context;

    public GetCelebrationsQueryHandler(CelebrationBoardContext context)
    {
      _context = context;
    }

    public Task<Celebration[]> Handle(GetCelebrationsQuery request, CancellationToken cancellationToken)
    {
      var celebrations = this._context.Set<Celebration>();
      return Task.FromResult(celebrations is not null ? celebrations.ToArray() : new Celebration[] { });
    }
  }
}

