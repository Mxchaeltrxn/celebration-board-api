namespace CelebrationBoard.Api.Celebrations.EditPrivacyLevel;

public partial class CelebrationsController : BaseController
{
  [ApiVersion("1.0")]
  [HttpPut("celebrations/{celebrationId}/edit/privacy-level")]
  [SwaggerOperation(
      Summary = "Update a single celebration.",
      Tags = new[] { "CelebrationEndpoints" })
  ]
  [SwaggerResponse(200, "Celebration was updated.", typeof(Celebration))]
  [SwaggerResponse(400, "One or more request fields are invalid.")]
  [SwaggerResponse(403, "You are not authorised to update this celebration.")]
  [SwaggerResponse(404, "The celebration with provided id could not be found, and therefore could not be updated.")]
  [SwaggerResponse(500, "Unexpected server error.")]
  public async Task<IActionResult> EditCelebrationPrivacyLevel(EditCelebrationPrivacyLevelRequest request)
  {
    var sendOrError = await base.Mediator.Send(new EditCelebrationPrivacyLevelCommand(
      userId: request.UserId,
      celebrationId: request.CelebrationId,
      request.PrivacyLevel
    ));

    if (sendOrError.IsFailure)
      return this.FromResult(sendOrError);

    return this.NoContent();
  }
}