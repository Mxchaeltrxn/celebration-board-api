using System.Net;
using CelebrationBoard.Domain.Common;
using MediatR;

namespace CelebrationBoard.Api.Common;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseController : ControllerBase
{
  private IMediator? _mediator;

  protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;

  protected new IActionResult Ok(object result)
  {
    return new EnvelopeResult(Envelope.Ok(result), HttpStatusCode.OK);
  }

  protected IActionResult Created(object result)
  {
    return new EnvelopeResult(Envelope.Ok(result), HttpStatusCode.Created);
  }

  protected new IActionResult NoContent()
  {
    return new EnvelopeResult(Envelope.Ok(null), HttpStatusCode.NoContent);
  }

  protected IActionResult FromResult(IUnitResult<Error> result)
  {
    if (result.IsSuccess)
      return Ok();

    if (result.Error == Errors.General.NotFound())
      return NotFound(Envelope.Error(result.Error, null));

    return BadRequest(Envelope.Error(result.Error, null));
  }
}