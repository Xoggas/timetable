using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Timetable.Api.LessonsSchedule.Dtos;
using Timetable.Api.LessonsSchedule.Entities;
using Timetable.Api.LessonsSchedule.Repositories;
using Timetable.Api.Tests.Integration.Shared;
using Common_DayOfWeek = Timetable.Api.LessonsSchedule.Common.DayOfWeek;
using DayOfWeek = Timetable.Api.LessonsSchedule.Common.DayOfWeek;

namespace Timetable.Api.Tests.Integration;

public sealed class LessonTableControllerTests_Post : IClassFixture<MongoDbFixture>
{
    private readonly HttpClient _client;
    private readonly ILessonTablesRepository _lessonTablesRepository;
    private readonly ILessonTablesBackupRepository _lessonTablesBackupRepository;

    public LessonTableControllerTests_Post(MongoDbFixture dbFixture)
    {
        var factory = new TimeTableWebApplicationFactory(dbFixture.Database);

        _lessonTablesRepository = factory.Services.GetRequiredService<ILessonTablesRepository>();

        _lessonTablesBackupRepository = factory.Services.GetRequiredService<ILessonTablesBackupRepository>();

        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_MakeBackup_WhenDayOfWeekIsInvalid_ShouldReturnBadRequest()
    {
        const Common_DayOfWeek dayOfWeek = (Common_DayOfWeek)999;

        var response = await _client.PostAsync($"api/lesson-table/{dayOfWeek}/backup", null);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_MakeBackup_WhenTableDoesntExist_ShouldCreateOneAndBackupIt()
    {
        const Common_DayOfWeek dayOfWeek = Common_DayOfWeek.Monday;

        var expectedBackup = new LessonTable
        {
            DayOfWeek = dayOfWeek,
            Lessons = []
        };

        Assert.False(await TableExists(dayOfWeek));

        var response = await _client.PostAsync($"api/lesson-table/{dayOfWeek}/backup", null);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var createdBackup = await _lessonTablesBackupRepository.GetByDayOfWeekAsync(dayOfWeek);

        Assert.NotNull(createdBackup);

        Assert.Equal(expectedBackup, createdBackup, (a, b) =>
            a.DayOfWeek == b.DayOfWeek &&
            a.Lessons.SequenceEqual(b.Lessons));
    }

    [Fact]
    public async Task Post_MakeBackup_WhenTableExists_ShouldReturnTheSameTable()
    {
        const Common_DayOfWeek dayOfWeek = Common_DayOfWeek.Tuesday;

        var updateLessonTableDto = new UpdateLessonTableDto
        {
            Lessons =
            [
                [
                    "lesson-before-backup"
                ]
            ]
        };

        var response = await _client.PutAsJsonAsync($"api/lesson-table/{dayOfWeek}", updateLessonTableDto);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.True(await TableExists(dayOfWeek));

        response = await _client.PostAsync($"api/lesson-table/{dayOfWeek}/backup", null);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var createdBackup = await _lessonTablesBackupRepository.GetByDayOfWeekAsync(dayOfWeek);

        Assert.NotNull(createdBackup);

        Assert.Equal(dayOfWeek, createdBackup.DayOfWeek);

        Assert.True(updateLessonTableDto.Lessons.SequenceEqual(createdBackup.Lessons));
    }

    [Fact]
    public async Task Post_RestoreBackup_WhenDayOfWeekIsInvalid_ShouldReturnBadRequest()
    {
        const Common_DayOfWeek dayOfWeek = (Common_DayOfWeek)999;

        var response = await _client.PostAsync($"api/lesson-table/{dayOfWeek}/restore", null);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_RestoreBackup_WhenBackupDoesntExist_ShouldReturnNotFound()
    {
        const Common_DayOfWeek dayOfWeek = Common_DayOfWeek.Wednesday;

        Assert.Null(await _lessonTablesBackupRepository.GetByDayOfWeekAsync(dayOfWeek));

        var response = await _client.PostAsync($"api/lesson-table/{dayOfWeek}/restore", null);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        
        Assert.Null(await _lessonTablesBackupRepository.GetByDayOfWeekAsync(dayOfWeek));
    }

    [Fact]
    public async Task Post_RestoreBackup_ShouldRestoreTheBackup()
    {
        const Common_DayOfWeek dayOfWeek = Common_DayOfWeek.Thursday;

        var updateLessonTableDtoBeforeBackup = new UpdateLessonTableDto
        {
            Lessons =
            [
                [
                    "lesson-before-backup"
                ]
            ]
        };

        var updateLessonTableDtoAfterBackup = new UpdateLessonTableDto
        {
            Lessons =
            [
                [
                    "lesson-after-backup"
                ]
            ]
        };

        var response = await _client.PutAsJsonAsync($"api/lesson-table/{dayOfWeek}", updateLessonTableDtoBeforeBackup);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        response = await _client.PostAsync($"api/lesson-table/{dayOfWeek}/backup", null);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        response = await _client.PutAsJsonAsync($"api/lesson-table/{dayOfWeek}", updateLessonTableDtoAfterBackup);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        await GetAndCompareTables(dayOfWeek, updateLessonTableDtoAfterBackup.Lessons);

        response = await _client.PostAsync($"api/lesson-table/{dayOfWeek}/restore", null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var restoredLessonTable = await response.Content.ReadFromJsonAsync<LessonTableDto>();

        Assert.NotNull(restoredLessonTable);

        Assert.Equal(dayOfWeek, restoredLessonTable.DayOfWeek);

        Assert.True(updateLessonTableDtoBeforeBackup.Lessons.SequenceEqual(restoredLessonTable.Lessons));

        await GetAndCompareTables(dayOfWeek, updateLessonTableDtoBeforeBackup.Lessons);
    }

    private async Task GetAndCompareTables(Common_DayOfWeek dayOfWeek, string[][] expectedLessonsCollection)
    {
        var lessonTable = await _client.GetFromJsonAsync<LessonTableDto>($"api/lesson-table/{dayOfWeek}");

        Assert.NotNull(lessonTable);

        Assert.Equal(dayOfWeek, lessonTable.DayOfWeek);

        Assert.True(expectedLessonsCollection.SequenceEqual(lessonTable.Lessons));
    }

    private async Task<bool> TableExists(Common_DayOfWeek dayOfWeek)
    {
        return await _lessonTablesRepository.GetAsync(dayOfWeek) is not null;
    }
}