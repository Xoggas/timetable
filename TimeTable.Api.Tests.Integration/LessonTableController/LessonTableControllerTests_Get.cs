using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using TimeTable.Api.LessonsSchedule.Dtos;
using TimeTable.Api.LessonsSchedule.Repositories;
using TimeTable.Api.Tests.Integration.Shared;
using DayOfWeek = TimeTable.Api.LessonsSchedule.Common.DayOfWeek;

namespace TimeTable.Api.Tests.Integration;

public sealed class LessonTableControllerTests_Get : IClassFixture<MongoDbFixture>
{
    private readonly HttpClient _client;
    private readonly ILessonTablesRepository _lessonTablesRepository;

    public LessonTableControllerTests_Get(MongoDbFixture dbFixture)
    {
        var factory = new TimeTableWebApplicationFactory(dbFixture.Database);

        _lessonTablesRepository = factory.Services.GetService<ILessonTablesRepository>()!;

        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_WhenDayOfWeekIsInvalid_ShouldReturnBadRequest()
    {
        const DayOfWeek dayOfWeek = (DayOfWeek)999;

        var response = await _client.GetAsync($"api/lesson-table/{dayOfWeek}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Get_WhenTableDoesntExist_ShouldReturnEmptyTable()
    {
        const DayOfWeek dayOfWeek = DayOfWeek.Monday;

        var expectedResult = new LessonTableDto
        {
            DayOfWeek = dayOfWeek,
            Lessons = []
        };

        Assert.False(await TableExists(dayOfWeek));

        await GetAndCompareTables(dayOfWeek, expectedResult.Lessons);
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