using CelebrationBoard.Domain.Authentication;

namespace CelebrationBoard.Application.Celebrations;

public sealed class PostCelebrationCommand : IRequest<Result<long, Error>>
{
  public long UserId { get; }
  public string Title { get; }
  public string Content { get; }
  public string AccessLevel { get; }

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
    private readonly ICurrentUserService _currentUserService;

    public PostCelebrationCommandHandler(CelebrationBoardContext context, ICurrentUserService currentUserService)
    {
      _context = context;
      this._currentUserService = currentUserService;
    }

    public Task<Result<long, Error>> Handle(PostCelebrationCommand request, CancellationToken cancellationToken)
    {
      var currentUserId = _currentUserService.UserId;
      var user = this._context.Set<User>().Find(request.UserId);
      if (user is null || user.UserId.ToString() != currentUserId)
        return Task.FromResult(Result.Failure<long, Error>(Errors.General.Unauthorised(request.UserId)));

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

