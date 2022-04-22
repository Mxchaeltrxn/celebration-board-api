namespace CelebrationBoard.Api.Celebrations.Delete;

public partial class CelebrationsController : BaseController
{
  [ApiVersion("1.0")]
  [HttpDelete]
  [SwaggerOperation(
       Summary = "Delete a post.",
       Description = "Delete an existing post.",
       Tags = new[] { "PostEndpoints" })
   ]
  [SwaggerResponse(204, "Post deleted.", typeof(Celebration))]
  [SwaggerResponse(400, "One or more payload fields are invalid.")]
  [SwaggerResponse(403, "You are not authorised to delete this post. Users can only delete this own posts.")]
  [SwaggerResponse(404, "Post with given id could not be found, and therefore could not be deleted.")]
  [SwaggerResponse(500, "Unexpected server error.")]
  public async Task<IActionResult> DeleteCelebration(DeleteCelebrationRequest request)
  {
    var sendOrError = await base.Mediator.Send(new DeleteCelebrationCommand(
       id: request.Id
     ));

    if (sendOrError.IsFailure)
      return this.FromResult(sendOrError);

    return this.NoContent();
  }
}