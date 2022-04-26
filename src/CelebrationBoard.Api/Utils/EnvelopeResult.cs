using System.Net;

namespace CelebrationBoard.Api.Utils;

public sealed class EnvelopeResult : IActionResult
{
  private readonly Envelope _envelope;
  private readonly int _statusCode;
  public EnvelopeResult(Envelope envelope, HttpStatusCode statusCode)
  {
    _statusCode = (int)statusCode;
    _envelope = envelope;
  }

  public Task ExecuteResultAsync(ActionContext context)
  {
    var objectResult = new ObjectResult(_envelope)
    {
      StatusCode = _statusCode
    };

    return objectResult.ExecuteResultAsync(context);
  }
}