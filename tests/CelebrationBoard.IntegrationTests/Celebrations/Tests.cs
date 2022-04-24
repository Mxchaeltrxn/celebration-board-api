using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using CelebrationBoard.Api.Authentication;
using CelebrationBoard.Api.Celebrations.Delete;
using CelebrationBoard.Api.Celebrations.GetAll;
using CelebrationBoard.Api.Celebrations.Post;
using CelebrationBoard.Api.Common;
using CelebrationBoard.Domain.Celebrations;
using CelebrationBoard.IntegrationTests.Common;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace CelebrationBoard.IntegrationTests.Celebrations;

public sealed class Tests : IntegrationTest
{
  public Tests(ApiWebApplicationFactory fixture) : base(fixture)
  {
  }

  private static PostCelebrationRequest ValidPostCelebrationRequest(long userId, string privacyLevel = "PRIVATE")
  {
    return new PostCelebrationRequest(
      UserId: userId,
      Title: Guid.NewGuid().ToString(),
      Content: Guid.NewGuid().ToString(),
      PrivacyLevel: privacyLevel
    );
  }

  private static string apiV1(string url)
  {
    return $"/api/v1/{url}";
  }

  public async Task<T> PostAndDeserialise<T, U>(string uri, U request, HttpStatusCode statusCode)
  {
    var response = await _client.PostAsJsonAsync(uri, request);
    response.StatusCode.Should().Be(statusCode);
    var resultString = await response.Content.ReadAsStringAsync();
    return JsonConvert.DeserializeObject<T>(resultString);
  }

  public async Task<T> GetAndDeserialise<T>(string uri, HttpStatusCode statusCode)
  {
    var response = await _client.GetAsync(uri);
    response.StatusCode.Should().Be(statusCode);
    var resultString = await response.Content.ReadAsStringAsync();
    return JsonConvert.DeserializeObject<T>(resultString);
  }

  public async Task<T> DeleteAndDeserialise<T, U>(string uri, U request, HttpStatusCode statusCode = HttpStatusCode.OK)
  {
    var message = new HttpRequestMessage
    {
      Method = HttpMethod.Delete,
      RequestUri = new Uri(uri),
      Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")
    };
    var response = await _client.SendAsync(message);
    response.StatusCode.Should().Be(statusCode);
    var resultString = await response.Content.ReadAsStringAsync();
    return JsonConvert.DeserializeObject<T>(resultString);
  }

  [Fact]
  public async Task Post_celebration_for_non_existent_user()
  {
    var postCelebrationRequest = ValidPostCelebrationRequest(100000L);
    var postCelebrationResponse = await PostAndDeserialise<Envelope<PostCelebrationResponse>, PostCelebrationRequest>(apiV1("celebrations"), postCelebrationRequest, HttpStatusCode.NotFound);

    postCelebrationResponse.Result.Should().Be(null);
    postCelebrationResponse.ErrorCode.Should().Be("record.not.found");
    postCelebrationResponse.ErrorMessage.Should().Be("Entity with type 'User' not found with Id '100000'");
    postCelebrationResponse.InvalidField.Should().Be(null);
  }

  [Fact]
  public async Task Post_celebration_and_get_it()
  {
    var registerRequest = new RegisterRequest(Guid.NewGuid().ToString(), $"{Guid.NewGuid()}@gmail.com", Guid.NewGuid().ToString());
    var registerResponse = await PostAndDeserialise<Envelope<long>, RegisterRequest>(apiV1("register"), registerRequest, HttpStatusCode.OK);

    var postCelebrationRequest = ValidPostCelebrationRequest(registerResponse.Result);
    var postCelebrationResponse = await PostAndDeserialise<Envelope<PostCelebrationResponse>, PostCelebrationRequest>(apiV1("celebrations"), postCelebrationRequest, HttpStatusCode.Created);

    var getAllResponse = await GetAndDeserialise<Envelope<GetCelebrationsResponse>>(apiV1("celebrations"), HttpStatusCode.OK);
    var celebration = getAllResponse.Result.Celebrations.Where(c => c.Id == postCelebrationResponse.Result.Id).FirstOrDefault();

    celebration.Should().NotBeNull();
    celebration.Id.Should().Be(postCelebrationResponse.Result.Id);
    celebration.Title.Should().Be(postCelebrationRequest.Title);
    celebration.Content.Should().Be(postCelebrationRequest.Content);
    celebration.PrivacyLevel.Should().Be(postCelebrationRequest.PrivacyLevel);
  }

  [Fact]
  public async Task Post_three_celebrations_and_get_them()
  {
    var registerRequest = new RegisterRequest(Guid.NewGuid().ToString(), $"{Guid.NewGuid()}@gmail.com", Guid.NewGuid().ToString());
    var registerResponse = await PostAndDeserialise<Envelope<long>, RegisterRequest>(apiV1("register"), registerRequest, HttpStatusCode.OK);

    var postCelebrationRequest = ValidPostCelebrationRequest(registerResponse.Result);
    var postCelebrationResponse = await PostAndDeserialise<Envelope<PostCelebrationResponse>, PostCelebrationRequest>(apiV1("celebrations"), postCelebrationRequest, HttpStatusCode.Created);
    var postCelebrationRequest1 = ValidPostCelebrationRequest(registerResponse.Result);
    var postCelebrationResponse1 = await PostAndDeserialise<Envelope<PostCelebrationResponse>, PostCelebrationRequest>(apiV1("celebrations"), postCelebrationRequest1, HttpStatusCode.Created);
    var postCelebrationRequest2 = ValidPostCelebrationRequest(registerResponse.Result);
    var postCelebrationResponse2 = await PostAndDeserialise<Envelope<PostCelebrationResponse>, PostCelebrationRequest>(apiV1("celebrations"), postCelebrationRequest2, HttpStatusCode.Created);

    var getAllResponse = await GetAndDeserialise<Envelope<GetCelebrationsResponse>>(apiV1("celebrations"), HttpStatusCode.OK);
    var celebrations = getAllResponse.Result.Celebrations.Where(c => new[] { postCelebrationResponse.Result.Id, postCelebrationResponse1.Result.Id, postCelebrationResponse2.Result.Id }.Contains(c.Id)).OrderBy(k => k.Id).ToArray();

    celebrations.Length.Should().Be(3);
    celebrations[0].Id.Should().Be(postCelebrationResponse.Result.Id);
    celebrations[0].Title.Should().Be(postCelebrationRequest.Title);
    celebrations[0].Content.Should().Be(postCelebrationRequest.Content);
    celebrations[0].PrivacyLevel.Should().Be(postCelebrationRequest.PrivacyLevel);

    celebrations[1].Id.Should().Be(postCelebrationResponse1.Result.Id);
    celebrations[1].Title.Should().Be(postCelebrationRequest1.Title);
    celebrations[1].Content.Should().Be(postCelebrationRequest1.Content);
    celebrations[1].PrivacyLevel.Should().Be(postCelebrationRequest1.PrivacyLevel);

    celebrations[2].Id.Should().Be(postCelebrationResponse2.Result.Id);
    celebrations[2].Title.Should().Be(postCelebrationRequest2.Title);
    celebrations[2].Content.Should().Be(postCelebrationRequest2.Content);
    celebrations[2].PrivacyLevel.Should().Be(postCelebrationRequest2.PrivacyLevel);
  }

  [Fact]
  public async Task Post_celebration_and_delete_it_cannot_be_retrieved()
  {
    var registerRequest = new RegisterRequest(Guid.NewGuid().ToString(), $"{Guid.NewGuid()}@gmail.com", Guid.NewGuid().ToString());
    var registerResponse = await PostAndDeserialise<Envelope<long>, RegisterRequest>(apiV1("register"), registerRequest, HttpStatusCode.OK);

    var postCelebrationRequest = ValidPostCelebrationRequest(registerResponse.Result);
    var postCelebrationResponse = await PostAndDeserialise<Envelope<PostCelebrationResponse>, PostCelebrationRequest>(apiV1("celebrations"), postCelebrationRequest, HttpStatusCode.Created);

    var deleteCelebrationRequest = new DeleteCelebrationRequest(postCelebrationResponse.Result.Id);
    var deleteCelebrationResponse = await DeleteAndDeserialise<Envelope, DeleteCelebrationRequest>(apiV1("celebrations"), deleteCelebrationRequest);

    var getAllResponse = await GetAndDeserialise<Envelope<GetCelebrationsResponse>>(apiV1("celebrations"), HttpStatusCode.OK);
    var celebration = getAllResponse.Result.Celebrations.Where(c => c.Id == postCelebrationResponse.Result.Id).FirstOrDefault();

    celebration.Should().BeNull();
  }

  [Fact]
  public async Task Delete_non_existent_celebration()
  {
    var deleteCelebrationRequest = new DeleteCelebrationRequest(100000L);
    var deleteCelebrationResponse = await DeleteAndDeserialise<Envelope<object?>, DeleteCelebrationRequest>(apiV1("celebrations"), deleteCelebrationRequest, HttpStatusCode.NotFound);

    deleteCelebrationResponse.Result.Should().BeNull();
    deleteCelebrationResponse.ErrorCode.Should().Be("record.not.found");
    deleteCelebrationResponse.ErrorMessage.Should().Be("Entity with type 'Celebration' not found with Id '100000'");
    deleteCelebrationResponse.InvalidField.Should().BeNull();
  }

  [Fact]
  public async Task Post_celebration_with_no_content_returns_bad_request()
  {
    var registerRequest = new RegisterRequest(Guid.NewGuid().ToString(), $"{Guid.NewGuid()}@gmail.com", Guid.NewGuid().ToString());
    var registerResponse = await PostAndDeserialise<Envelope<long>, RegisterRequest>(apiV1("register"), registerRequest, HttpStatusCode.OK);

    var postCelebrationRequest = new PostCelebrationRequest(
      UserId: registerResponse.Result,
      Title: Guid.NewGuid().ToString(),
      Content: "",
      PrivacyLevel: "private"
    );
    var postCelebrationResponse = await PostAndDeserialise<Envelope<PostCelebrationResponse>, PostCelebrationRequest>(apiV1("celebrations"), postCelebrationRequest, HttpStatusCode.BadRequest);
    postCelebrationResponse.Result.Should().Be(null);
    postCelebrationResponse.ErrorCode.Should().Be("value.is.required");
    postCelebrationResponse.ErrorMessage.Should().Be("Value is required");
    postCelebrationResponse.InvalidField.Should().Be("Content");
  }

  // [Fact]
  // public async Task Post_celebration_with_invalid_privacy_level_fails()
  // {
  //   var registerRequest = new RegisterRequest(Guid.NewGuid().ToString(), $"{Guid.NewGuid()}@gmail.com", Guid.NewGuid().ToString());
  //   var registerResponse = await PostAndDeserialise<Envelope<long>, RegisterRequest>(apiV1("register"), registerRequest, HttpStatusCode.OK);

  //   var postCelebrationRequest = ValidPostCelebrationRequest(registerResponse.Result);
  //   var postCelebrationResponse = await PostAndDeserialise<Envelope<PostCelebrationResponse>, PostCelebrationRequest>(apiV1("celebrations"), postCelebrationRequest, HttpStatusCode.Created);

  //   var getAllResponse = await GetAndDeserialise<Envelope<GetCelebrationsResponse>>(apiV1("celebrations"), HttpStatusCode.OK);
  //   var celebration = getAllResponse.Result.Celebrations.Where(c => c.Id == postCelebrationResponse.Result.Id).FirstOrDefault();

  //   celebration.Should().NotBeNull();
  //   celebration.Id.Equals(postCelebrationResponse.Result.Id);
  //   celebration.Title.Equals("valid title", StringComparison.Ordinal);
  //   celebration.Content.Equals("valid content", StringComparison.Ordinal);
  //   celebration.PrivacyLevel.Equals("private", StringComparison.Ordinal);
  // }

  // [Fact]
  // public async Task Post_celebration_and_delete_it()
  // {
  //   var registerRequest = new RegisterRequest(Guid.NewGuid().ToString(), $"{Guid.NewGuid()}@gmail.com", Guid.NewGuid().ToString());
  //   var registerResponse = await PostAndDeserialise<Envelope<long>, RegisterRequest>(apiV1("register"), registerRequest, HttpStatusCode.OK);

  //   var postCelebrationRequest = ValidPostCelebrationRequest(registerResponse.Result);
  //   var postCelebrationResponse = await PostAndDeserialise<Envelope<PostCelebrationResponse>, PostCelebrationRequest>(apiV1("celebrations"), postCelebrationRequest, HttpStatusCode.Created);

  //   var getAllResponse = await GetAndDeserialise<Envelope<GetCelebrationsResponse>>(apiV1("celebrations"), HttpStatusCode.OK);
  //   var celebration = getAllResponse.Result.Celebrations.Where(c => c.Id == postCelebrationResponse.Result.Id).FirstOrDefault();

  //   celebration.Should().NotBeNull();
  //   celebration.Id.Equals(postCelebrationResponse.Result.Id);
  //   celebration.Title.Equals("valid title", StringComparison.Ordinal);
  //   celebration.Content.Equals("valid content", StringComparison.Ordinal);
  //   celebration.PrivacyLevel.Equals("private", StringComparison.Ordinal);
  // }

  // [Fact]
  // public async Task Delete_non_existent_celebration()
  // {
  //   var registerRequest = new RegisterRequest(Guid.NewGuid().ToString(), $"{Guid.NewGuid()}@gmail.com", Guid.NewGuid().ToString());
  //   var registerResponse = await PostAndDeserialise<Envelope<long>, RegisterRequest>(apiV1("register"), registerRequest, HttpStatusCode.OK);

  //   var postCelebrationRequest = ValidPostCelebrationRequest(registerResponse.Result);
  //   var postCelebrationResponse = await PostAndDeserialise<Envelope<PostCelebrationResponse>, PostCelebrationRequest>(apiV1("celebrations"), postCelebrationRequest, HttpStatusCode.Created);

  //   var getAllResponse = await GetAndDeserialise<Envelope<GetCelebrationsResponse>>(apiV1("celebrations"), HttpStatusCode.OK);
  //   var celebration = getAllResponse.Result.Celebrations.Where(c => c.Id == postCelebrationResponse.Result.Id).FirstOrDefault();

  //   celebration.Should().NotBeNull();
  //   celebration.Id.Equals(postCelebrationResponse.Result.Id);
  //   celebration.Title.Equals("valid title", StringComparison.Ordinal);
  //   celebration.Content.Equals("valid content", StringComparison.Ordinal);
  //   celebration.PrivacyLevel.Equals("private", StringComparison.Ordinal);
  // }
}

// Tests
// Create but user isn't the correct one when you try create for someone else
// get non existent
// Create + edit PL + get
// Create + edit content + get
// Create + edit content to invalid + get
// Create + edit title + get
// Create + toggle favourite + get => Toggle from false to true, true to false
// Create + toggle ditto + get => Toggle from false to true, true to false
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