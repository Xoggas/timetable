using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using TimeTable.Api.LessonsSchedule.Dtos;
using TimeTable.Api.LessonsSchedule.Repositories;
using TimeTable.Api.Tests.Integration.Shared;
using DayOfWeek = TimeTable.Api.LessonsSchedule.Common.DayOfWeek;

namespace TimeTable.Api.Tests.Integration;

public sealed class LessonTableControllerTests_Put : IClassFixture<MongoDbFixture>
{
    private const string VeryLongString = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

    private readonly HttpClient _client;
    private readonly ILessonTablesRepository _lessonTablesRepository;

    public LessonTableControllerTests_Put(MongoDbFixture dbFixture)
    {
        var factory = new TimeTableWebApplicationFactory(dbFixture.Database);

        _lessonTablesRepository = factory.Services.GetRequiredService<ILessonTablesRepository>();

        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Put_WhenDayOfWeekIsInvalid_ShouldReturnBadRequest()
    {
        const DayOfWeek dayOfWeek = (DayOfWeek)999;

        var updateLessonTableDto = new UpdateLessonTableDto
        {
            Lessons =
            [
                [
                    "updated-lesson"
                ]
            ]
        };

        var response = await _client.PutAsJsonAsync($"api/lesson-table/{dayOfWeek}", updateLessonTableDto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(VeryLongString)]
    public async Task Put_WhenDtoIsInvalid_ShouldReturnBadRequest(string lesson)
    {
        const DayOfWeek dayOfWeek = DayOfWeek.Tuesday;

        var updateLessonTableDto = new UpdateLessonTableDto
        {
            Lessons =
            [
                [
                    lesson
                ]
            ]
        };

        var response = await _client.PutAsJsonAsync($"api/lesson-table/{dayOfWeek}", updateLessonTableDto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_WhenTableDoesntExist_ShouldCreateUpdateIt()
    {
        const DayOfWeek dayOfWeek = DayOfWeek.Tuesday;

        var updateLessonTableDto = new UpdateLessonTableDto
        {
            Lessons =
            [
                [
                    "freshly-created-table"
                ]
            ]
        };

        Assert.False(await TableExists(dayOfWeek));

        var response = await _client.PutAsJsonAsync($"api/lesson-table/{dayOfWeek}", updateLessonTableDto);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        await GetAndCompareTables(dayOfWeek, updateLessonTableDto.Lessons);
    }

    [Fact]
    public async Task Put_WhenTableExists_ShouldUpdateIt()
    {
        const DayOfWeek dayOfWeek = DayOfWeek.Wednesday;

        var updateLessonTableDto = new UpdateLessonTableDto
        {
            Lessons =
            [
                [
                    "freshly-created-table"
                ]
            ]
        };

        var response = await _client.GetAsync($"api/lesson-table/{dayOfWeek}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await _client.PutAsJsonAsync($"api/lesson-table/{dayOfWeek}", updateLessonTableDto);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        await GetAndCompareTables(dayOfWeek, updateLessonTableDto.Lessons);
    }

    private async Task GetAndCompareTables(DayOfWeek dayOfWeek, string[][] expectedLessonsCollection)
    {
        var lessonTable = await _client.GetFromJsonAsync<LessonTableDto>($"api/lesson-table/{dayOfWeek}");

        Assert.NotNull(lessonTable);
        Assert.Equal(dayOfWeek, lessonTable.DayOfWeek);
        Assert.Equal(expectedLessonsCollection, lessonTable.Lessons);
    }

    private async Task<bool> TableExists(DayOfWeek dayOfWeek)
    {
        return await _lessonTablesRepository.GetAsync(dayOfWeek) is not null;
    }
}