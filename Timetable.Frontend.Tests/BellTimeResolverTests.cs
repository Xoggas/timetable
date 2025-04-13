using Timetable.Frontend.BellsSchedule.Helpers;
using Timetable.Frontend.BellsSchedule.Models;

namespace Timetable.Frontend.Tests;

public class BellTimeResolverTests
{
    private readonly List<BellTableRow> _rows =
    [
        new()
        {
            StartTime = new TimeModel(08, 50),
            EndTime = new TimeModel(09, 50),
        },
        new()
        {
            StartTime = new TimeModel(09, 55),
            EndTime = new TimeModel(10, 40),
        }
    ];

    [Fact]
    public void TimeBeforeAllLessons_ShouldReturnFirstRow_LessonsNotStarted()
    {
        var row = BellTimeResolver.ResolveRowAndState(_rows, 8 * 60 + 40, out var state);
        
        Assert.Equal(_rows[0], row);
        
        Assert.Equal(LessonState.LessonsNotStarted, state);
    }
    
    [Fact]
    public void TimeInRangeOfFirstLesson_ShouldReturnFirstRow_LessonIsGoing()
    {
        var row = BellTimeResolver.ResolveRowAndState(_rows, 8 * 60 + 50, out var state);
        
        Assert.Equal(_rows[0], row);
        
        Assert.Equal(LessonState.LessonIsGoing, state);
    }
    
    [Fact]
    public void TimeAfterFirstLessonAndBeforeSecondLesson_ShouldReturnSecondRow_NextLessonWillStartSoon()
    {
        var row = BellTimeResolver.ResolveRowAndState(_rows, 9 * 60 + 52, out var state);
        
        Assert.Equal(_rows[1], row);
        
        Assert.Equal(LessonState.NextLessonWillStartSoon, state);
    }
    
    [Fact]
    public void TimeAfterAllLessons_ShouldReturnFirst_LessonsEnded()
    {
        var row = BellTimeResolver.ResolveRowAndState(_rows, 10 * 60 + 40, out var state);
        
        Assert.Equal(_rows[0], row);
        
        Assert.Equal(LessonState.LessonsEnded, state);
    }
}