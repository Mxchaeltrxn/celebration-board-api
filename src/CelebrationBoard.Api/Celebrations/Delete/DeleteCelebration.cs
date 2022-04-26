namespace CelebrationBoard.Api.Celebrations.Delete;

public partial class CelebrationsController : BaseController
{
  [ApiVersion("1.0")]
  [HttpDelete("celebrations/{celebrationId}")]
  [SwaggerOperation(
       Summary = "Delete a celebration.",
       Tags = new[] { "CelebrationEndpoints" })
   ]
  [SwaggerResponse(204, "Celebration deleted.", typeof(Celebration))]
  [SwaggerResponse(400, "One or more request fields are invalid.")]
  [SwaggerResponse(403, "You are not authorised to delete this celebration. Users can only delete their own celebrations.")]
  [SwaggerResponse(404, "Celebration with given id could not be found, and therefore could not be deleted.")]
  [SwaggerResponse(500, "Unexpected server error.")]
  public async Task<IActionResult> DeleteCelebration(DeleteCelebrationRequest request)
  {
    var sendOrError = await base.Mediator.Send(new DeleteCelebrationCommand(
       userId: request.UserId,
       celebrationId: request.CelebrationId
     ));

    if (sendOrError.IsFailure)
      return this.FromResult(sendOrError);

    return this.Ok();
  }
}