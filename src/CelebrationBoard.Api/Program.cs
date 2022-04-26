using CelebrationBoard.Application.Utils;
using CelebrationBoard.Infrastructure.Utils;
using CelebrationBoard.Persistence.Utils;
using Serilog;

try
{
  var builder = WebApplication.CreateBuilder(args);

  builder.Host.UseSerilog((context, loggerConfig) => loggerConfig
      .ReadFrom.Configuration(context.Configuration));

  builder.Services.AddApplicationDependencies();
  builder.Services.AddPersistenceDependencies(builder.Configuration);
  builder.Services.AddInfrastructureDependencies(builder.Configuration);
  builder.Services.BootstrapApi(builder.Configuration);

  var app = builder.Build();

  app.Services.InitPersistenceDependencies((Microsoft.AspNetCore.Hosting.IHostingEnvironment)app.Environment);
  app.Services.InitInfrastructureDependencies(app.Environment);

  if (app.Environment.IsDevelopment())
  {
    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();

    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
  }
  app.UseSerilogRequestLogging(); // should be the first thing you configure on `app`.

  app.UseHttpsRedirection();

  app.UseAuthentication();
  app.UseAuthorization();

  app.MapControllers();

  app.UseMiddleware<ExceptionHandlerMiddleware>();
  app.UseMiddleware<RequestResponseLoggingMiddleware>();

  Log.Information("Started app with base urls: '{BaseUrls}'", builder.Configuration["Urls"]);

  app.Run();
}
catch (Exception ex)
{
  Log.Error(ex, "Something went wrong");
}
finally
{
  Log.CloseAndFlush();
}
// https://docs.microsoft.com/en-us/aspnet/core/migration/50-to-60-samples?view=aspnetcore-6.0#test-with-webapplicationfactory-or-testserver
public partial class Program { }
