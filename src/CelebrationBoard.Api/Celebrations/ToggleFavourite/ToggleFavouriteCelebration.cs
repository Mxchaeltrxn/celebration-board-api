using CelebrationBoard.Api.Celebrations.ToggleFavourite;

namespace CelebrationBoard.Api.Celebrations;

public sealed class CelebrationsController : BaseController
{
  [ApiVersion("1.0")]
  [HttpPut("celebrations/{celebrationId}/toggle-favourite")]
  [SwaggerOperation(
        Summary = "Update a single celebration.",
        Tags = new[] { "CelebrationEndpoints" })
    ]
  [SwaggerResponse(200, "Celebration favourite field toggled.", typeof(Celebration))]
  [SwaggerResponse(400, "One or more request fields are invalid.")]
  [SwaggerResponse(403, "You are not authorised to update this celebration.")]
  [SwaggerResponse(404, "The celebration with provided id could not be found, and therefore could not be updated.")]
  [SwaggerResponse(500, "Unexpected server error.")]
  public async Task<IActionResult> ToggleCelebrationFavourite(ToggleFavouriteCelebrationRequest request)
  {
    var sendOrError = await base.Mediator.Send(new ToggleCelebrationFavouriteCommand(
      userId: request.UserId,
      celebrationId: request.CelebrationId
    ));

    if (sendOrError.IsFailure)
      return this.FromResult(sendOrError);

    return this.NoContent();
  }
}
