using AutoFixture;
using Moq;
using TimeTableBackend.LessonsSchedule.Entities;
using TimeTableBackend.LessonsSchedule.Repositories;
using TimeTableBackend.LessonsSchedule.Services;
using DayOfWeek = TimeTableBackend.LessonsSchedule.Common.DayOfWeek;

namespace TimeTableBackend.Tests.Unit;

public sealed class LessonTablesServiceTests
{
    private readonly Mock<ILessonTablesRepository> _lessonTablesRepositoryMock;
    private readonly Mock<ILessonTablesBackupRepository> _lessonTablesBackupRepositoryMock;
    private readonly Mock<IEventService> _eventServiceMock;
    private readonly Fixture _fixture;
    private readonly LessonTablesService _lessonTablesService;

    public LessonTablesServiceTests()
    {
        _lessonTablesRepositoryMock = new Mock<ILessonTablesRepository>();
        _lessonTablesBackupRepositoryMock = new Mock<ILessonTablesBackupRepository>();
        _eventServiceMock = new Mock<IEventService>();
        _fixture = new Fixture();
        _lessonTablesService = new LessonTablesService(_lessonTablesRepositoryMock.Object,
            _lessonTablesBackupRepositoryMock.Object, _eventServiceMock.Object);
    }

    [Fact]
    public async Task GetLessonTableAsync_ShouldReturnLessonTable()
    {
        var lessonTable = _fixture.Create<LessonTable>();
        var dayOfWeek = lessonTable.DayOfWeek;

        _lessonTablesRepositoryMock
            .Setup(x => x.GetByDayOfWeekAsync(It.IsAny<DayOfWeek>()))
            .ReturnsAsync(lessonTable);

        var result = await _lessonTablesService.GetLessonTableByDayOfWeekAsync(dayOfWeek);

        Assert.Equal(lessonTable, result);
    }

    [Fact]
    public async Task UpdateLessonTableAsync_ShouldCallLessonTableRepositoryAndEventService()
    {
        var lessonTableEntity = _fixture.Create<LessonTable>();

        await _lessonTablesService.UpdateLessonTableAsync(lessonTableEntity);

        _lessonTablesRepositoryMock.Verify(x => x.UpdateAsync(It.Is<LessonTable>(y => y == lessonTableEntity)));

        _eventServiceMock.Verify(x => x.NotifyAllClientsAboutUpdate());
    }

    [Fact]
    public async Task MakeLessonTableBackupAsync()
    {
        var lessonTableEntityToBackup = _fixture.Create<LessonTable>();
        var dayOfWeek = lessonTableEntityToBackup.DayOfWeek;

        _lessonTablesRepositoryMock
            .Setup(x => x.GetByDayOfWeekAsync(It.IsAny<DayOfWeek>()))
            .ReturnsAsync(lessonTableEntityToBackup);

        await _lessonTablesService.MakeLessonTableBackupAsync(dayOfWeek);

        _lessonTablesBackupRepositoryMock.Verify(x =>
            x.CreateAsync(It.Is<LessonTable>(y => y.Equals(lessonTableEntityToBackup))));
    }

    [Fact]
    public async Task RestoreLessonFromBackupAsync_WhenBackupDoesntExist_ShouldReturnNull()
    {
        var dayOfWeek = _fixture.Create<DayOfWeek>();

        _lessonTablesBackupRepositoryMock
            .Setup(x => x.GetByDayOfWeekAsync(It.IsAny<DayOfWeek>()))
            .ReturnsAsync(default(LessonTable));

        var result = await _lessonTablesService.RestoreLessonTableFromBackupAsync(dayOfWeek);

        Assert.Null(result);
    }

    [Fact]
    public async Task RestoreLessonFromBackupAsync_WhenBackupExists_ShouldCallUpdateLessonTableAsync()
    {
        var lessonTableEntityBackup = _fixture.Create<LessonTable>();
        var dayOfWeek = lessonTableEntityBackup.DayOfWeek;

        _lessonTablesBackupRepositoryMock
            .Setup(x => x.GetByDayOfWeekAsync(It.IsAny<DayOfWeek>()))
            .ReturnsAsync(lessonTableEntityBackup);

        var result = await _lessonTablesService.RestoreLessonTableFromBackupAsync(dayOfWeek);

        _lessonTablesRepositoryMock.Verify(x => x.UpdateAsync(It.Is<LessonTable>(y => y == lessonTableEntityBackup)));

        _eventServiceMock.Verify(x => x.NotifyAllClientsAboutUpdate());

        Assert.Equal(result, lessonTableEntityBackup);
    }
}