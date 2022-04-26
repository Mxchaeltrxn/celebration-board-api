namespace CelebrationBoard.Api.Celebrations.Edit;

public partial class CelebrationsController : BaseController
{
  // [Authorize]
  [ApiVersion("1.0")]
  [HttpPut("celebrations/{celebrationId}/edit")]
  [SwaggerOperation(
      Summary = "Create a celebration.",
      Tags = new[] { "CelebrationEndpoints" })
  ]
  [SwaggerResponse(201, "Celebration content or title edited.", typeof(Celebration))]
  [SwaggerResponse(400, "One or more request fields are invalid, and therefore the celebration could not be created.")]
  // [SwaggerResponse(403, "You are not authorised to create a post.")]
  [SwaggerResponse(500, "Unexpected server error.")]
  public async Task<IActionResult> EditCelebration(EditCelebrationRequest request)
  {
    var sendOrError = await base.Mediator.Send(new EditCelebrationCommand(
      userId: request.UserId,
      celebrationId: request.CelebrationId,
      title: request.Title,
      content: request.Content
    ));

    if (sendOrError.IsFailure)
      return this.FromResult(sendOrError);

    return this.NoContent();
  }
}