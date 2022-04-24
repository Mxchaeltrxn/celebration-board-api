namespace CelebrationBoard.Persistence.Utils;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;


public static class Dependencies
{
  public static void Init(IServiceProvider serviceProvider, IWebHostEnvironment environment)
  {
    if (environment.IsDevelopment())
    {
      var celebrationBoardContext = serviceProvider.GetRequiredService<CelebrationBoardContext>();
      celebrationBoardContext.Database.EnsureDeleted();
      celebrationBoardContext.Database.EnsureCreated();
    }
    else
    {
      throw new NotImplementedException("Handle migrations in production for celebration board context");
    }
  }
  public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
  {
    services.AddScoped(_ => new CelebrationBoardContext(
      connectionString: configuration["CelebrationBoardConnectionString"],
      canLogToConsole: true
    ));
  }
}
