namespace Timetable.Api.BellsSchedule.Entities;

public sealed class TimeState
{
    public LessonState LessonState { get; init; }
    public int? RowIndex { get; init; }
    public BellTableRow? Row { get; init; }
    public int? PreviousRowIndex { get; init; }
    public BellTableRow? PreviousRow { get; init; }

    public TimeState(LessonState lessonState, int? rowIndex = null, BellTableRow? row = null,
        int? previousRowIndex = null, BellTableRow? previousRow = null)
    {
        LessonState = lessonState;
        RowIndex = rowIndex;
        Row = row;
        PreviousRowIndex = previousRowIndex;
        PreviousRow = previousRow;
    }
}