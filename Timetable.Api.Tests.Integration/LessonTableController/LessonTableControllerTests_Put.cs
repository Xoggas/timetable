using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Timetable.Api.LessonsSchedule.Common;
using Timetable.Api.LessonsSchedule.Dtos;
using Timetable.Api.LessonsSchedule.Repositories;
using Timetable.Api.Tests.Integration.Shared;

namespace Timetable.Api.Tests.Integration;

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
        const CustomDayOfWeek dayOfWeek = (CustomDayOfWeek)999;

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
    [InlineData(VeryLongString)]
    public async Task Put_WhenDtoIsInvalid_ShouldReturnBadRequest(string lesson)
    {
        const CustomDayOfWeek dayOfWeek = CustomDayOfWeek.Tuesday;

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
        const CustomDayOfWeek dayOfWeek = CustomDayOfWeek.Tuesday;

        var updateLessonTableDto = new UpdateLessonTableDto
        {
            Lessons =
            [
                [
                    "created-table"
                ],
                [
                    "created-table"
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
        const CustomDayOfWeek dayOfWeek = CustomDayOfWeek.Wednesday;

        var updateLessonTableDto = new UpdateLessonTableDto
        {
            Lessons =
            [
                [
                    "created-table"
                ],
                [
                    "created-table"
                ]
            ]
        };

        var response = await _client.GetAsync($"api/lesson-table/{dayOfWeek}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await _client.PutAsJsonAsync($"api/lesson-table/{dayOfWeek}", updateLessonTableDto);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        await GetAndCompareTables(dayOfWeek, updateLessonTableDto.Lessons);
    }

    private async Task GetAndCompareTables(CustomDayOfWeek customDayOfWeek, string[][] expectedLessonsCollection)
    {
        var lessonTable = await _client.GetFromJsonAsync<LessonTableDto>($"api/lesson-table/{customDayOfWeek}");

        Assert.NotNull(lessonTable);
        Assert.Equal(customDayOfWeek, lessonTable.CustomDayOfWeek);
        Assert.Equal(expectedLessonsCollection, lessonTable.Lessons);
    }

    private async Task<bool> TableExists(CustomDayOfWeek customDayOfWeek)
    {
        return await _lessonTablesRepository.GetAsync(customDayOfWeek) is not null;
    }
}