using System.Net;
using System.Text.Json;
using CelebrationBoard.Domain.Common;

namespace CelebrationBoard.Api.Common;

public sealed class ExceptionHandler
{
  private readonly RequestDelegate _next;
  private readonly IWebHostEnvironment _env;

  public ExceptionHandler(RequestDelegate next, IWebHostEnvironment env)
  {
    _next = next;
    _env = env;
  }

  public async Task Invoke(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Exception ex)
    {
      await HandleException(context, ex);
    }
    
    Task HandleException(HttpContext context, Exception exception)
    {
      var errorMessage = _env.IsProduction() ? "Internal server error" : $"Exception: {exception.Message}";
      var error = Errors.General.InternalServerError(errorMessage);
      var envelope = Envelope.Error(error, null);
      var result = JsonSerializer.Serialize(envelope);

      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
      return context.Response.WriteAsync(result);
    }
  }

}