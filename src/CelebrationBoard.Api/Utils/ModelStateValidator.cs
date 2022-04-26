using System.Net;
using CelebrationBoard.Domain.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public class ModelStateValidator
{
  public static IActionResult ValidateModelState(ActionContext context)
  {
    (string fieldName, ModelStateEntry? entry) = context.ModelState.First(x => x.Value!.Errors.Count > 0);
    string errorSerialized = entry!.Errors.First().ErrorMessage;

    Error error = Error.Deserialise(errorSerialized);
    var envelope = Envelope.Error(error, fieldName);
    var envelopeResult = new EnvelopeResult(envelope, HttpStatusCode.BadRequest);

    return envelopeResult;
  }
}