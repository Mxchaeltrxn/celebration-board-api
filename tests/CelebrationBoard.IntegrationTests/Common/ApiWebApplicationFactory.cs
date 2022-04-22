namespace CelebrationBoard.IntegrationTests.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
  public IConfiguration Configuration { get; private set; } = default!;
  protected override IHost CreateHost(IHostBuilder builder)
  {
    builder.ConfigureHostConfiguration(config =>
        {
          Configuration = new ConfigurationBuilder()
                .AddJsonFile(path: "integrationTestSettings.json", optional: false)
                .Build();

          config.AddConfiguration(Configuration);
        });

    return base.CreateHost(builder);
  }
}
