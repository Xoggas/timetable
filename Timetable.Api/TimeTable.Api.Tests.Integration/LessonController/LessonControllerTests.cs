using System.Net;
using System.Net.Http.Json;
using Timetable.Api.LessonsSchedule.Dtos;
using Timetable.Api.Tests.Integration.Shared;

namespace Timetable.Api.Tests.Integration;

public sealed class LessonControllerTests : IClassFixture<MongoDbFixture>
{
    private readonly HttpClient _client;

    public LessonControllerTests(MongoDbFixture dbFixture)
    {
        _client = new TimeTableWebApplicationFactory(dbFixture.Database)
            .CreateClient();
    }

    [Fact]
    public async Task Get_ShouldReturnCollectionOfOneLesson()
    {
        await CreateTempLesson();

        var response = await _client.GetAsync("api/lesson");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<IEnumerable<LessonDto>>();

        Assert.NotNull(content);

        var lesson = content.First();
        
        Assert.Equal(string.Empty, lesson.Name);
    }

    [Fact]
    public async Task Post_ShouldCreateLesson()
    {
        var createdLesson = await CreateTempLesson();

        var response = await _client.GetAsync("api/lesson");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var lessonsFromDatabase = await response.Content.ReadFromJsonAsync<IEnumerable<LessonDto>>();

        Assert.NotNull(lessonsFromDatabase);

        Assert.Single(lessonsFromDatabase, x =>
            x.Id == createdLesson.Id && x.Name == createdLesson.Name);
    }

    [Fact]
    public async Task Put_WhenIdIsInvalid_ShouldReturnNotFound()
    {
        var updateLessonDto = new UpdateLessonDto
        {
            Name = "updated_lesson"
        };

        const string id = "invalid_id";

        var response = await _client.PutAsJsonAsync($"api/lesson/{id}", updateLessonDto);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Put_WhenDtoIsInvalid_ShouldReturnBadRequest()
    {
        var updateLessonDto = new UpdateLessonDto
        {
            Name = new string('a', 50)
        };

        const string id = "some_id";

        var response = await _client.PutAsJsonAsync($"api/lesson/{id}", updateLessonDto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_WhenEverythingIsValid_ShouldUpdateLesson()
    {
        var createdLesson = await CreateTempLesson();

        var lessonId = createdLesson.Id;

        var updateLessonDto = new UpdateLessonDto
        {
            Name = "updated_lesson"
        };

        var response = await _client.PutAsJsonAsync($"api/lesson/{lessonId}", updateLessonDto);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var updatedLessonsCollection = await _client.GetFromJsonAsync<IEnumerable<LessonDto>>($"api/lesson");

        Assert.NotNull(updatedLessonsCollection);

        Assert.Contains(updatedLessonsCollection, x =>
            x.Id == lessonId && x.Name == updateLessonDto.Name);
    }

    [Fact]
    public async Task Delete_WhenIdIsInvalid_ShouldReturnNotFound()
    {
        const string id = "invalid_id";

        var response = await _client.DeleteAsync($"api/lesson/{id}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_WhenIdIsValid_ShouldDeleteLesson()
    {
        var createdLesson = await CreateTempLesson();

        var response = await _client.DeleteAsync($"api/lesson/{createdLesson.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var updatedLessonsCollection = await _client.GetFromJsonAsync<IEnumerable<LessonDto>>("api/lesson");

        Assert.NotNull(updatedLessonsCollection);

        Assert.DoesNotContain(updatedLessonsCollection, x =>
            x.Id == createdLesson.Id);
    }

    private async Task<LessonDto> CreateTempLesson()
    {
        var response = await _client.PostAsJsonAsync<LessonDto>("api/lesson", null);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdLessonDto = await response.Content.ReadFromJsonAsync<LessonDto>();

        Assert.NotNull(createdLessonDto);

        return createdLessonDto;
    }
}