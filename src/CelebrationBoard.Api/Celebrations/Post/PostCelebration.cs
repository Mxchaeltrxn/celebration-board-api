using CelebrationBoard.Api.Celebrations.Post;

namespace CelebrationBoard.Api.Celebrations.PostCelebration;

public partial class CelebrationsController : BaseController
{
  // [Authorize]
  [ApiVersion("1.0")]
  [HttpPost]
  [SwaggerOperation(
      Summary = "Create a post.",
      Description = "Create a new post.",
      Tags = new[] { "PostEndpoints" })
  ]
  [SwaggerResponse(201, "Post created.", typeof(Celebration))]
  [SwaggerResponse(400, "One or more payload fields are invalid, and therefore the post could not be created.")]
  // [SwaggerResponse(403, "You are not authorised to create a post.")]
  [SwaggerResponse(500, "Unexpected server error.")]
  public async Task<IActionResult> PostCelebration(PostCelebrationRequest request)
  {
    var sendOrError = await base.Mediator.Send(new PostCelebrationCommand(
      userId: request.UserId,
      title: request.Title,
      content: request.Content,
      accessLevel: request.PrivacyLevel
    ));

    if (sendOrError.IsFailure)
      return this.FromResult(sendOrError);

    return this.Created(new PostCelebrationResponse(sendOrError.Value));
  }
}