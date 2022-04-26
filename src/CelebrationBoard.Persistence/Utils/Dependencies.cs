namespace CelebrationBoard.Persistence.Utils;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class Dependencies
{
  // TODO: Unsure why IWebHostEnvironment can't be used as a type for `environment`, but it can be used in Infrastructure/Dependencies.cs
  // Apparently IHostingEnvironment will be deprecated later.
  public static void InitPersistenceDependencies(this IServiceProvider services, IHostingEnvironment environment)
  {
    if (environment.IsDevelopment())
    {
      using var scope = services.CreateScope();
      var serviceProvider = scope.ServiceProvider;

      var celebrationBoardContext = serviceProvider.GetRequiredService<CelebrationBoardContext>();
      celebrationBoardContext.Database.EnsureDeleted();
      celebrationBoardContext.Database.EnsureCreated();
    }
    else
    {
      throw new NotImplementedException("Handle migrations in production for celebration board context");
    }
  }
  public static void AddPersistenceDependencies(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped(_ => new CelebrationBoardContext(
      connectionString: configuration["CelebrationBoardConnectionString"],
      canLogToConsole: true
    ));
  }
}
