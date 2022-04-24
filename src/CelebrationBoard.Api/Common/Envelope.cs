using CelebrationBoard.Domain.Common;

namespace CelebrationBoard.Api.Common;

public class Envelope
{
  public object? Result { get; }
  public string? ErrorCode { get; }
  public string? ErrorMessage { get; }
  public string? InvalidField { get; }
  public DateTime TimeGenerated { get; }

  public Envelope(object? result, Error? error, string? invalidField)
  {
    Result = result;
    ErrorCode = error?.Code;
    ErrorMessage = error?.Message;
    InvalidField = invalidField;
    TimeGenerated = DateTime.UtcNow;
  }

  public static Envelope Ok(object? result = null)
  {
    return new Envelope(result, null, null);
  }

  public static Envelope Error(Error error, string? invalidField)
  {
    return new Envelope(null, error, invalidField);
  }
}

public class Envelope<T>
{
  public T? Result { get; set; }
  public string? ErrorCode { get; set; }
  public string? ErrorMessage { get; set; }
  public string? InvalidField { get; set; }
  public DateTime TimeGenerated { get; set; }


}