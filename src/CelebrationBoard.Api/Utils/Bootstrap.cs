using System.Text.Json.Serialization;
using CelebrationBoard.Api.Celebrations.Post;
using FluentValidation.AspNetCore;

namespace CelebrationBoard.Api.Utils;
public static class Bootstrap
{
  public static void BootstrapApi(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddRouting(options =>
    {
      options.LowercaseUrls = true;
    });
    services.AddCors(options =>
    {
      options.AddPolicy(name: Constants.SpecificOriginsPolicy,
        corsBuilder =>
            corsBuilder
              .WithOrigins(configuration["Cors:AllowedOrigins"])
              .WithHeaders(configuration["Cors:AllowedHeaders"])
              .WithMethods(configuration["Cors:AllowedMethods"])
        );
    });

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(config =>
    {
      config.EnableAnnotations();
    });

    services.AddControllers()
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
          options.RegisterValidatorsFromAssemblyContaining<ToggleFavouriteCelebrationRequestValidator>();
        });
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    // Api Versioning
    // https://dev.to/moesmp/what-every-asp-net-core-web-api-project-needs-part-2-api-versioning-and-swagger-3nfm
    services.AddApiVersioning(options => options.ReportApiVersions = true);

    services.AddVersionedApiExplorer(options =>
    {
      options.GroupNameFormat = "'v'VVV";
      options.SubstituteApiVersionInUrl = true;
    });

  }
}