namespace Timetable.Frontend.BellsSchedule.Models;

public sealed class TimeState
{
    public LessonState LessonState { get; init; }
    public int? RowIndex { get; init; }
    public BellTableRow? Row { get; init; }
    public int? PreviousRowIndex { get; init; }
    public BellTableRow? PreviousRow { get; init; }
}