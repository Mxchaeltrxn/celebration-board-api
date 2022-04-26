// namespace CelebrationBoard.Api.Celebrations.ToggleDitto;

// public partial class CelebrationsController : BaseController
// {
//   [ApiVersion("1.0")]
//   [HttpPut("/api/v{version:apiVersion}/posts")]
//   [SwaggerOperation(
//        Summary = "Update a single post.",
//        Description = "Idempotently update a post.",
//        Tags = new[] { "CelebrationEndpoints" })
//    ]
//   [SwaggerResponse(200, "The product was updated.", typeof(Celebration))]
//   [SwaggerResponse(400, "One or more payload fields are invalid.")]
//   [SwaggerResponse(403, "You are not authorised to update this post.")]
//   [SwaggerResponse(404, "The post with provided id could not be found, and therefore could not be updated.")]
//   [SwaggerResponse(500, "Unexpected server error.")]
//   public async Task<IActionResult> ToggleCelebrationDitto(ToggleCelebrationDittoRequest request)
//   {
//     var sendOrError = await base.Mediator.Send(new ToggleCelebrationDittoCommand(
//       celebrationId: request.CelebrationId,
//       userId: request.UserId
//     ));

//     if (sendOrError.IsFailure)
//       return this.FromResult(sendOrError);

//     return this.NoContent();
//   }
// }