namespace CelebrationBoard.Domain.Common;

public sealed class Error : ValueObject
{
  private const string Separator = "||";
  public string Code { get; }
  public string Message { get; }

  public Error(string code, string message)
  {
    Code = code;
    Message = message;
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Code;
  }

  public string Serialise()
  {
    return $"{Code}{Separator}{Message}";
  }

  public static Error Deserialise(string serialised)
  {
    if (serialised == "A non=empty request body is required")
      return Errors.General.ValueIsRequired();

    var data = serialised.Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries);

    if (data.Length < 2)
      throw new Exception($"Invalid error serialisation: '{serialised}");

    return new Error(data[0], data[1]);
  }
}

public static class Errors
{
  public static class User
  {
    public static Error EmailIsTaken() =>
        new Error("user.email.is.taken", "User email is taken");

    public static Error UsernameIsTaken() =>
        new Error("user.username.is.taken", "User username is taken");

    public static Error InvalidCredentials() =>
        new Error("user.credentials.invalid", "User credentials do not pass security requirements for user creation");


  }

  public static class General
  {
    public static Error Unauthorised(long? userId = null)
    {
      string ofUser = userId == null ? "" : $" of user with Id '{userId}'";
      return new Error("user.does.not.have.required.permissions", $"User does not have permission to access the requested resources{ofUser}");
    }

    public static Error NotFound(string? entityType = null, long? id = null)
    {
      string withId = id == null ? "" : $" with Id '{id}'";
      var withEntityType = entityType == null ? "" : $" with type '{entityType}'";
      return new Error("record.not.found", $"Entity{withEntityType} not found{withId}");
    }

    public static Error ValueIsInvalid() =>
        new Error("value.is.invalid", "Value is invalid");

    public static Error GreaterThan(int greaterThan) =>
        new Error("value.is.required", $"Value must be greater than '{greaterThan}'");

    public static Error InvalidLength(string? name = null)
    {
      string label = name == null ? " " : " " + name + " ";
      return new Error("invalid.string.length", $"Invalid{label}length");
    }

    public static Error ValueIsRequired() =>
      new Error("value.is.required", "Value is required");

    public static Error InternalServerError(string message)
    {
      return new Error("internal.server.error", message);
    }
  }
}