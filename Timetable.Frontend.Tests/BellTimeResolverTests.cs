using Timetable.Frontend.BellsSchedule.Helpers;
using Timetable.Frontend.BellsSchedule.Models;

namespace Timetable.Frontend.Tests;

public class BellTimeResolverTests
{
    private readonly List<BellTableRow> _rows =
    [
        new()
        {
            StartTime = new TimeModel(08, 20),
            EndTime = new TimeModel(08, 35),
        },
        new()
        {
            StartTime = new TimeModel(08, 40),
            EndTime = new TimeModel(09, 20),
        },
        new()
        {
            StartTime = new TimeModel(09, 35),
            EndTime = new TimeModel(10, 15),
        },
        new()
        {
            StartTime = new TimeModel(10, 30),
            EndTime = new TimeModel(11, 10),
        },
        new()
        {
            StartTime = new TimeModel(11, 20),
            EndTime = new TimeModel(12, 00),
        },
        new()
        {
            StartTime = new TimeModel(12, 10),
            EndTime = new TimeModel(12, 50),
        },
        new()
        {
            StartTime = new TimeModel(13, 10),
            EndTime = new TimeModel(13, 50),
        },
        new()
        {
            StartTime = new TimeModel(14, 10),
            EndTime = new TimeModel(14, 50),
        },
        new()
        {
            StartTime = new TimeModel(15, 00),
            EndTime = new TimeModel(15, 40),
        }
    ];

    [Theory]
    [InlineData(08, 10, 0, LessonState.BeforeLessons)]
    [InlineData(08, 20, 0, LessonState.LessonIsGoing)]
    [InlineData(08, 35, 0, LessonState.Break)]
    [InlineData(08, 36, 0, LessonState.Break)]
    [InlineData(08, 40, 1, LessonState.LessonIsGoing)]
    [InlineData(15, 40, 8, LessonState.AfterLessons)]
    public void Tests(int hour, int minute, int expectedRowIndex, LessonState expectedState)
    {
        var row = BellTimeResolver.ResolveRowAndState(_rows, hour * 60 + minute, out var state);
        
        Assert.Equal(_rows[expectedRowIndex], row);
        
        Assert.Equal(expectedState, state);
    }
}