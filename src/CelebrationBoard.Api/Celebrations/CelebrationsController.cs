namespace CelebrationBoard.Api.Celebrations;

public class CelebrationsController : BaseController
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
  public async Task<IActionResult> PostCelebrationAsync(PostCelebrationRequest request)
  {
    await base.Mediator.Send(new PostCelebrationCommand(
      title: request.Title,
      content: request.Content,
      accessLevel: request.PrivacyLevel
    ));

    return this.Created("/api/v1/celebrations", null);
  }
}
