namespace CelebrationBoard.Persistence.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class Dependencies
{
  public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
  {
    services.AddScoped(_ => new CelebrationBoardContext(
      connectionString: configuration["CelebrationBoardConnectionString"],
      canLogToConsole: true
    ));
  }
}
