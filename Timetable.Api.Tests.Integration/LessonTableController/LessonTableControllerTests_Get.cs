using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Timetable.Api.LessonsSchedule.Common;
using Timetable.Api.LessonsSchedule.Dtos;
using Timetable.Api.LessonsSchedule.Repositories;
using Timetable.Api.Tests.Integration.Extensions;
using Timetable.Api.Tests.Integration.Shared;

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
        const CustomDayOfWeek dayOfWeek = (CustomDayOfWeek)999;

        var response = await _client.GetAsync($"api/lesson-table/{dayOfWeek}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Get_WhenTableDoesntExist_ShouldReturnEmptyTable()
    {
        const CustomDayOfWeek dayOfWeek = CustomDayOfWeek.Monday;

        var expectedResult = new LessonTableDto
        {
            CustomDayOfWeek = dayOfWeek,
            Lessons = _emptyLessonTable
        };

        Assert.False(await TableExists(dayOfWeek));

        var lessonTable = await _client.GetFromJsonAsync<LessonTableDto>($"api/lesson-table/{dayOfWeek}");

        Assert.NotNull(lessonTable);

        Assert.Equal(expectedResult, lessonTable, (a, b) =>
            a.CustomDayOfWeek == b.CustomDayOfWeek &&
            a.Lessons.StringArraysEqual(b.Lessons));
    }

    private async Task<bool> TableExists(CustomDayOfWeek customDayOfWeek)
    {
        return await _lessonTablesRepository.GetAsync(customDayOfWeek) is not null;
    }
}