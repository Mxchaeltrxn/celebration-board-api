using System.Net.Http.Json;
using System.Threading.Tasks;
using CelebrationBoard.Api.Celebrations.GetAll;
using CelebrationBoard.Api.Celebrations.Post;
using CelebrationBoard.IntegrationTests.Common;
using System.Linq;
using CelebrationBoard.Domain.Celebrations;
using System;
using Newtonsoft.Json;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using CelebrationBoard.Api.Common;
using CSharpFunctionalExtensions;

namespace CelebrationBoard.IntegrationTests.Celebrations;

public sealed class Tests : IntegrationTest
{
  public Tests(ApiWebApplicationFactory fixture) : base(fixture)
  {
  }

  private static PostCelebrationRequest ValidPostCelebrationRequest(long userId, string privacyLevel = "PRIVATE")
  {
    return new PostCelebrationRequest(
      UserId: 1,
      Title: "Valid title",
      Content: "Valid content",
      PrivacyLevel: privacyLevel
    );
  }

  private static string apiV1(string url)
  {
    return $"/api/v1/{url}";
  }

  [Fact]
  public async Task Post_celebration_and_get_it()
  {
    using var scope = _factory.Services.GetService<IServiceProvider>()!
                                       .CreateScope();
    var context = scope.ServiceProvider.GetService<CelebrationBoardContext>();
    context.Set<User>().Add(new User(1L));
    context.SaveChanges();
    var postCelebrationRequest = ValidPostCelebrationRequest(1L);
    var postCelebrationResponse = await _client.PostAsJsonAsync(apiV1("celebrations"), postCelebrationRequest);
    postCelebrationResponse.StatusCode.Should().Be(HttpStatusCode.Created);
    var resultString = await postCelebrationResponse.Content.ReadAsStringAsync();
    var result = JsonConvert.DeserializeObject<Envelope<PostCelebrationResponse>>(resultString);

    // result.Id.Equals(1L);
    var getAllResponse = await _client.GetAsync(apiV1("celebrations"));
    var a = await getAllResponse.Content.ReadAsStringAsync();
    var r = JsonConvert.DeserializeObject<Envelope<GetCelebrationsResponse>>(a);
    var aaa = r.Result.Celebrations.Where(c => c.Id == result.Result.Id).FirstOrDefault();

    aaa.Should().NotBeNull();
    aaa.Id.Equals(result.Result.Id);
    aaa.Title.Equals("valid title", StringComparison.Ordinal);
    aaa.Content.Equals("valid content", StringComparison.Ordinal);
    aaa.PrivacyLevel.Equals("private", StringComparison.Ordinal);
  }
}

// Tests
// Create + get
// Create + delete + get (get non existent)
// Create + edit PL + get
// Create + edit content + get
// Create + edit content to invalid + get
// Create + edit title + get
// Create + toggle favourite + get => Toggle from false to true, true to false
// Create + toggle ditto + get => Toggle from false to true, true to false
// Create multiple + get all
// Delete non existent
// Create but user doesn't exist
// Delete but user doesn't exist
// Edits but user doesn't exist
// Get all - pagination (test length and expected results) --> Filter by a unique identifier that you added to the post content, for example. Or just filter based on the posts you just made and then order those.
// Get all - sorting
// Get all - filtering

// Test privacy levels (must be authorised and the user who owns the celebration to view it)
// Public can be viewed by user and other users

// Think of more business cases

// Test email uniqueness in the user db.
// Test can't have more than 5 drafts at a time rule when that gets released.