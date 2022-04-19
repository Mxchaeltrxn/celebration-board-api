namespace CelebrationBoard.Application.Celebrations;

public sealed class PostCelebrationCommand : IRequest
{
  public readonly string Title;
  public readonly string Content;
  public readonly string AccessLevel;

  public PostCelebrationCommand(string title, string content, string accessLevel)
  {
    Title = title;
    Content = content;
    AccessLevel = accessLevel;
  }
}

public sealed class PostCelebrationCommandHandler : IRequestHandler<PostCelebrationCommand>
{
  private readonly CelebrationBoardContext _context;

  public PostCelebrationCommandHandler(CelebrationBoardContext context)
  {
    _context = context;
  }

  public async Task<Unit> Handle(PostCelebrationCommand request, CancellationToken cancellationToken)
  {
    var title = Title.Create(request.Title).Value;
    var content = Content.Create(request.Content).Value;
    var privacyLevel = PrivacyLevel.Create(request.AccessLevel).Value;
    var celebration2 = new Celebration(title, content, privacyLevel);

    var tagNames = content.CalculateTags();
    var alreadyPersistedTags = _context.Set<CelebrationTag>().Where(t => tagNames.Contains(t.Name));
    var notYetPersistedTags = tagNames
      .Where(t => !alreadyPersistedTags.Select(ct => ct.Name).Contains(t))
      .Select(t =>
      {
        var tag = new CelebrationTag(t);
        tag.AddCelebration(celebration2);
        return tag;
      })
      ;
    celebration2.AddTags(notYetPersistedTags.Concat(alreadyPersistedTags).ToList());
    _context.Set<Celebration>().Add(celebration2);
    _context.Set<CelebrationTag>().AddRange(notYetPersistedTags);

    _context.SaveChanges();

    return Unit.Value;
  }
}