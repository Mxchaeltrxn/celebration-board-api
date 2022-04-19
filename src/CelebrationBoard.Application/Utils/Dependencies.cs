namespace CelebrationBoard.Application.Utils;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class Dependencies
{
  public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
  {
    services.AddMediatR(Assembly.GetExecutingAssembly());
  }
}
