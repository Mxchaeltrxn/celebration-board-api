namespace CelebrationBoard.Api.Celebrations.Edit;

public partial class CelebrationsController : BaseController
{
  // [Authorize]
  [ApiVersion("1.0")]
  [HttpPut("{celebrationId}/edit/privacy-level")]
  [SwaggerOperation(
      Summary = "Create a post.",
      Description = "Create a new post.",
      Tags = new[] { "PostEndpoints" })
  ]
  [SwaggerResponse(201, "Post created.", typeof(Celebration))]
  [SwaggerResponse(400, "One or more payload fields are invalid, and therefore the post could not be created.")]
  // [SwaggerResponse(403, "You are not authorised to create a post.")]
  [SwaggerResponse(500, "Unexpected server error.")]
  public async Task<IActionResult> EditCelebration(EditCelebrationRequest request)
  {
    var sendOrError = await base.Mediator.Send(new EditCelebrationCommand(
      id: request.Id,
      title: request.Title,
      content: request.Content
    ));

    if (sendOrError.IsFailure)
      return this.FromResult(sendOrError);

    return this.NoContent();
  }
}