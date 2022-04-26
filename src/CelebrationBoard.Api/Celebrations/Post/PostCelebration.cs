using CelebrationBoard.Api.Celebrations.Post;

namespace CelebrationBoard.Api.Celebrations.PostCelebration;

public partial class CelebrationsController : BaseController
{
  [ApiVersion("1.0")]
  [HttpPost("celebrations")]
  [SwaggerOperation(
      Summary = "Create a celebration.",
      Tags = new[] { "CelebrationEndpoints" })
  ]
  [SwaggerResponse(201, "Celebration created.", typeof(Celebration))]
  [SwaggerResponse(400, "One or more request fields are invalid, and therefore the celebration could not be created.")]
  [SwaggerResponse(403, "You are not authorised to create a post.")]
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