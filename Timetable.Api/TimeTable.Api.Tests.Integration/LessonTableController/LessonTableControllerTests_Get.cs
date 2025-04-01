using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Timetable.Api.LessonsSchedule.Dtos;
using Timetable.Api.LessonsSchedule.Repositories;
using Timetable.Api.Tests.Integration.Extensions;
using Timetable.Api.Tests.Integration.Shared;
using Common_DayOfWeek = Timetable.Api.LessonsSchedule.Common.DayOfWeek;
using DayOfWeek = Timetable.Api.LessonsSchedule.Common.DayOfWeek;

namespace Timetable.Api.Tests.Integration;

public sealed class LessonTableControllerTests_Get : IClassFixture<MongoDbFixture>
{
    private readonly HttpClient _client;
    private readonly ILessonTablesRepository _lessonTablesRepository;
    private readonly string[][] _emptyLessonTable =
    [
        [
            string.Empty
        ],
        [
            string.Empty
        ]
    ];

    public LessonTableControllerTests_Get(MongoDbFixture dbFixture)
    {
        var factory = new TimeTableWebApplicationFactory(dbFixture.Database);

        _lessonTablesRepository = factory.Services.GetRequiredService<ILessonTablesRepository>();

        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_WhenDayOfWeekIsInvalid_ShouldReturnBadRequest()
    {
        const Common_DayOfWeek dayOfWeek = (Common_DayOfWeek)999;

        var response = await _client.GetAsync($"api/lesson-table/{dayOfWeek}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Get_WhenTableDoesntExist_ShouldReturnEmptyTable()
    {
        const Common_DayOfWeek dayOfWeek = Common_DayOfWeek.Monday;

        var expectedResult = new LessonTableDto
        {
            DayOfWeek = dayOfWeek,
            Lessons = _emptyLessonTable
        };

        Assert.False(await TableExists(dayOfWeek));

        var lessonTable = await _client.GetFromJsonAsync<LessonTableDto>($"api/lesson-table/{dayOfWeek}");

        Assert.NotNull(lessonTable);

        Assert.Equal(expectedResult, lessonTable, (a, b) =>
            a.DayOfWeek == b.DayOfWeek &&
            a.Lessons.StringArraysEqual(b.Lessons));
    }

    private async Task<bool> TableExists(Common_DayOfWeek dayOfWeek)
    {
        return await _lessonTablesRepository.GetAsync(dayOfWeek) is not null;
    }
}