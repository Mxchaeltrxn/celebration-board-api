namespace CelebrationBoard.IntegrationTests.Common;

using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Respawn;

public abstract class IntegrationTest : IClassFixture<ApiWebApplicationFactory>
{
  private readonly Checkpoint _checkpoint = new Checkpoint
  {
    SchemasToInclude = new[] {
            "public"
        },
    WithReseed = true,
    DbAdapter = DbAdapter.Postgres
  };

  protected readonly ApiWebApplicationFactory _factory;
  protected readonly HttpClient _client;

  protected readonly CelebrationBoardContext _celebrationBoardContext;

  public IntegrationTest(ApiWebApplicationFactory fixture)
  {
    _factory = fixture;
    _client = _factory.CreateClient();

    using var conn = new NpgsqlConnection(fixture.Configuration["CelebrationBoardConnectionString"]);

    _celebrationBoardContext = new CelebrationBoardContext(fixture.Configuration["CelebrationBoardConnectionString"], false);

    _celebrationBoardContext.Database.EnsureDeleted();
    _celebrationBoardContext.Database.EnsureCreated();
    // Workaround because async await can't be used in constructors.
    // https://stackoverflow.com/questions/23048285/call-asynchronous-method-in-constructor
    conn.OpenAsync().Wait();
    _checkpoint.Reset(conn).Wait();
  }
}
