namespace CelebrationBoard.Application.Celebrations;

public sealed class PostCelebrationCommand : IRequest<Result<long, Error>>
{
  public readonly long UserId;
  public readonly string Title;
  public readonly string Content;
  public readonly string AccessLevel;

  public PostCelebrationCommand(long userId, string title, string content, string accessLevel)
  {
    UserId = userId;
    Title = title;
    Content = content;
    AccessLevel = accessLevel;
  }

  internal sealed class PostCelebrationCommandHandler : IRequestHandler<PostCelebrationCommand, Result<long, Error>>
  {
    private readonly CelebrationBoardContext _context;

    public PostCelebrationCommandHandler(CelebrationBoardContext context)
    {
      _context = context;
    }

    public Task<Result<long, Error>> Handle(PostCelebrationCommand request, CancellationToken cancellationToken)
    {
      var user = this._context.Set<User>().Find(request.UserId);
      if (user is null)
        return Task.FromResult(Result.Failure<long, Error>(Errors.General.NotFound(nameof(User), request.UserId)));

      var title = Domain.Celebrations.Title.Create(request.Title).Value;
      var content = Domain.Celebrations.Content.Create(request.Content).Value;
      var privacyLevel = PrivacyLevel.Create(request.AccessLevel).Value;
      var celebration = new Celebration(user, title, content, privacyLevel);

      var tagNames = content.CalculateTags();
      var alreadyPersistedTags = _context.Set<Tag>().Where(t => tagNames.Contains(t.Name));
      var notYetPersistedTags = tagNames
        .Where(t => !alreadyPersistedTags.Select(ct => ct.Name).Contains(t))
        .Select(t =>
        {
          var tag = new Tag(t);
          tag.AddCelebration(celebration);
          return tag;
        })
        ;
      celebration.AddTags(notYetPersistedTags.Concat(alreadyPersistedTags).ToList());
      _context.Set<Celebration>().Add(celebration);
      _context.Set<Tag>().AddRange(notYetPersistedTags);

      // user.PostCelebration(celebration);

      _context.SaveChanges();

      return Task.FromResult(Result.Success<long, Error>(celebration.Id));
    }
  }
}

