using System.Text;
using System.Text.Json.Serialization;
using CelebrationBoard.Api.Celebrations.Post;
using FluentValidation.AspNetCore;
using Serilog;

try
{
  var builder = WebApplication.CreateBuilder(args);

  CelebrationBoard.Application.Utils.Dependencies.ConfigureServices(builder.Configuration, builder.Services);
  CelebrationBoard.Persistence.Utils.Dependencies.ConfigureServices(builder.Configuration, builder.Services);
  CelebrationBoard.Infrastructure.Utils.Dependencies.ConfigureServices(builder.Configuration, builder.Services);

  builder.Host.UseSerilog((context, loggerConfig) => loggerConfig
      .ReadFrom.Configuration(context.Configuration));

  builder.Services.AddCors(options =>
  {
    options.AddPolicy(name: Constants.SpecificOriginsPolicy,
      corsBuilder =>
          corsBuilder
            .WithOrigins(builder.Configuration["Cors:AllowedOrigins"])
            .WithHeaders(builder.Configuration["Cors:AllowedHeaders"])
            .WithMethods(builder.Configuration["Cors:AllowedMethods"])
      );
  });

  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen(config =>
  {
    config.EnableAnnotations();
  });

  builder.Services.AddControllers()
          .AddJsonOptions(
              options =>
              {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
              }
          )
          .ConfigureApiBehaviorOptions(options =>
      {
        options.InvalidModelStateResponseFactory = ModelStateValidator.ValidateModelState;
      })
      .AddFluentValidation(options =>
      {
        options.RegisterValidatorsFromAssemblyContaining<PostCelebrationRequestValidator>();
      });
  // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen();

  // Api Versioning
  // https://dev.to/moesmp/what-every-asp-net-core-web-api-project-needs-part-2-api-versioning-and-swagger-3nfm
  builder.Services.AddApiVersioning(options => options.ReportApiVersions = true);

  builder.Services.AddVersionedApiExplorer(options =>
  {
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
  });

  var app = builder.Build();

  using var scope = app.Services.CreateScope();
  var serviceProvider = scope.ServiceProvider;

  CelebrationBoard.Persistence.Utils.Dependencies.Init(serviceProvider, (Microsoft.AspNetCore.Hosting.IHostingEnvironment)app.Environment);
  CelebrationBoard.Infrastructure.Utils.Dependencies.Init(serviceProvider, app.Environment);

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
