using Timetable.Api.BellsSchedule.Entities;

namespace Timetable.Api.BellsSchedule.Dtos;

public sealed class TimeStateDto
{
    public LessonState LessonState { get; init; }
    public int? RowIndex { get; init; }
    public BellTableRowDto? Row { get; init; }
    public int? PreviousRowIndex { get; init; }
    public BellTableRowDto? PreviousRow { get; init; }
}