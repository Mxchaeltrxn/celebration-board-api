using System.Text.Json.Serialization;
using CelebrationBoard.Api.Celebrations.Post;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

CelebrationBoard.Application.Utils.Dependencies.ConfigureServices(builder.Configuration, builder.Services);
CelebrationBoard.Persistence.Utils.Dependencies.ConfigureServices(builder.Configuration, builder.Services);

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
var forumContext = scope.ServiceProvider.GetRequiredService<CelebrationBoardContext>();
forumContext.Database.EnsureDeleted();

forumContext.Database.EnsureCreated();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandler>();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// https://docs.microsoft.com/en-us/aspnet/core/migration/50-to-60-samples?view=aspnetcore-6.0#test-with-webapplicationfactory-or-testserver
public partial class Program { }
