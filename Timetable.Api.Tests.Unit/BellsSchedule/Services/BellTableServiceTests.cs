using MongoDB.Driver;
using Moq;
using Timetable.Api.BellsSchedule.BackgroundServices;
using Timetable.Api.BellsSchedule.Entities;
using Timetable.Api.BellsSchedule.Services;
using Timetable.Api.Shared.Services;

namespace Timetable.Api.Tests.BellsSchedule.Services;

public class BellTableServiceTests
{
    private readonly Mock<IMongoDbService> _mongoDbServiceMock;
    private readonly Mock<IMongoCollection<BellTable>> _bellTableCollectionMock;
    private readonly Mock<IBellsScheduleEventService> _bellsScheduleEventServiceMock;
    private readonly Mock<IBellTableSharedEventBus> _bellTableSharedEventBusMock;
    private readonly Mock<ITimeProvider> _timeProviderMock;
    private readonly BellTableService _service;

    private readonly BellTable _bellTable = new()
    {
        Rows =
        [
            new BellTableRow(08, 00, 09, 00),
            new BellTableRow(09, 10, 09, 50),
            new BellTableRow(10, 00, 11, 00),
        ]
    };

    public BellTableServiceTests()
    {
        _mongoDbServiceMock = new Mock<IMongoDbService>();
        _bellTableCollectionMock = new Mock<IMongoCollection<BellTable>>();
        _bellsScheduleEventServiceMock = new Mock<IBellsScheduleEventService>();
        _bellTableSharedEventBusMock = new Mock<IBellTableSharedEventBus>();
        _timeProviderMock = new Mock<ITimeProvider>();
        _service = new BellTableService(_mongoDbServiceMock.Object, _bellsScheduleEventServiceMock.Object,
            _bellTableSharedEventBusMock.Object, _timeProviderMock.Object);
    }

    [Fact]
    public void GetTimeState_EmptyTable_ShouldReturnNoneState()
    {
        var bellTable = new BellTable();
        var timeState = _service.GetTimeState(bellTable);

        Assert.Equal(LessonState.None, timeState.LessonState);
    }

    [Theory]
    [InlineData(07, 00)]
    [InlineData(07, 59)]
    public void GetTimeState_FilledTable_ShouldReturnBeforeLessons(int hour, int minute)
    {
        _timeProviderMock
            .Setup(x => x.TotalMinutes)
            .Returns(hour * 60 + minute);

        var timeState = _service.GetTimeState(_bellTable);

        Assert.Equal(LessonState.BeforeLessons, timeState.LessonState);
    }

    [Theory]
    [InlineData(11, 00)]
    [InlineData(11, 01)]
    public void GetTimeState_FilledTable_ShouldReturnAfterLessons(int hour, int minute)
    {
        _timeProviderMock
            .Setup(x => x.TotalMinutes)
            .Returns(hour * 60 + minute);

        var timeState = _service.GetTimeState(_bellTable);

        Assert.Equal(LessonState.AfterLessons, timeState.LessonState);
    }

    [Theory]
    [InlineData(08, 00, 0)]
    [InlineData(08, 01, 0)]
    [InlineData(08, 59, 0)]
    [InlineData(09, 10, 1)]
    [InlineData(09, 11, 1)]
    [InlineData(09, 49, 1)]
    [InlineData(10, 00, 2)]
    [InlineData(10, 01, 2)]
    [InlineData(10, 59, 2)]
    public void GetTimeState_FilledTable_ShouldReturnLessonIsGoing(int hour, int minute, int expectedRow)
    {
        var currentTimeInMinutes = hour * 60 + minute;

        _timeProviderMock
            .Setup(x => x.TotalMinutes)
            .Returns(currentTimeInMinutes);

        var timeState = _service.GetTimeState(_bellTable);

        Assert.Equal(LessonState.LessonIsGoing, timeState.LessonState);
        Assert.Equal(expectedRow, timeState.RowIndex);
        Assert.True(currentTimeInMinutes >= timeState.Row?.StartTime.TotalMinutes &&
                    currentTimeInMinutes < timeState.Row?.EndTime.TotalMinutes);
    }

    [Theory]
    [InlineData(09, 00, 1, 0)]
    [InlineData(09, 01, 1, 0)]
    [InlineData(09, 09, 1, 0)]
    [InlineData(09, 50, 2, 1)]
    [InlineData(09, 51, 2, 1)]
    [InlineData(09, 59, 2, 1)]
    public void GetTimeState_FilledTable_ShouldReturnBreak(int hour, int minute, int currentRow, int previousRow)
    {
        var currentTimeInMinutes = hour * 60 + minute;

        _timeProviderMock
            .Setup(x => x.TotalMinutes)
            .Returns(currentTimeInMinutes);

        var timeState = _service.GetTimeState(_bellTable);

        Assert.Equal(LessonState.Break, timeState.LessonState);
        Assert.Equal(currentRow, timeState.RowIndex);
        Assert.Equal(previousRow, timeState.PreviousRowIndex);
    }
}