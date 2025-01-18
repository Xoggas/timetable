using System.Net;
using System.Net.Http.Json;
using TimeTable.Api.LessonsSchedule.Dtos;
using TimeTable.Api.Tests.Integration.Shared;

namespace TimeTable.Api.Tests.Integration;

public sealed class LessonControllerTests : IClassFixture<MongoDbFixture>
{
    private readonly HttpClient _client;

    public LessonControllerTests(MongoDbFixture dbFixture)
    {
        _client = new TimeTableWebApplicationFactory(dbFixture.Database)
            .CreateClient();
    }

    [Fact]
    public async Task Get_ShouldReturnEmptyCollection()
    {
        var response = await _client.GetAsync("api/lesson");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<IEnumerable<LessonDto>>();

        const string expectedName = "test_lesson";

        Assert.Equal(expectedName, content?.First().Name);
    }

    [Fact]
    public async Task Post_WhenNameIsInvalid_ShouldReturnBadRequest()
    {
        var createLessonDto = new CreateLessonDto
        {
            Name = new string('a', 50)
        };

        var response = await _client.PostAsJsonAsync("api/lesson", createLessonDto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_WhenNameIsValid_ShouldCreateLesson()
    {
        var createLessonDto = new CreateLessonDto
        {
            Name = "created_lesson"
        };

        var response = await _client.PostAsJsonAsync("api/lesson", createLessonDto);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadFromJsonAsync<LessonDto>();
        
        Assert.Equal(createLessonDto.Name, content?.Name);
    }
}