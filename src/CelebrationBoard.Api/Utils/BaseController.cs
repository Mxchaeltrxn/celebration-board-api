using System.Net;
using CelebrationBoard.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace CelebrationBoard.Api.Utils;

[Authorize]
[ApiController]
[Route("api/v{version:apiVersion}")]
public abstract class BaseController : ControllerBase
{
  private IMediator? _mediator;

  protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;

  protected new IActionResult Ok(object? result = null)
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

    if (result.Error == Errors.User.InvalidCredentials())
      return Unauthorized(Envelope.Error(result.Error, null));

    if (result.Error == Errors.User.EmailIsTaken() || result.Error == Errors.User.EmailIsTaken())
      return base.StatusCode((int)HttpStatusCode.Conflict, Envelope.Error(result.Error, null));

    return BadRequest(Envelope.Error(result.Error, null));
  }
}