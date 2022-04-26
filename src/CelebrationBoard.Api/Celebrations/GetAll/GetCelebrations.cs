namespace CelebrationBoard.Api.Celebrations.GetAll;

public partial class CelebrationsController : BaseController
{
  [AllowAnonymous]
  [ApiVersion("1.0")]
  [HttpGet("celebrations")]
  [SwaggerOperation(
    Summary = "Gets a list of celebrations",
    Tags = new[] { "CelebrationEndpoints" })
  ]
  [SwaggerResponse(200, "Celebrations retrieved.", typeof(Celebration))]
  [SwaggerResponse(400, "One or more request fields are invalid.")]
  [SwaggerResponse(500, "Unexpected server error.")]
  public async Task<IActionResult> GetCelebrations()
  {
    var celebrations = await Mediator.Send(new GetCelebrationsQuery());
    var celebrationsDtos = celebrations.Select(MapCelebrationToDto).ToArray();
    return this.Ok(new GetCelebrationsResponse(celebrationsDtos));

    static CelebrationDto MapCelebrationToDto(Celebration celebration) => new CelebrationDto(
        celebration.Id,
        celebration.Title,
        celebration.Content,
        celebration.AccessLevel
      );
  }
}