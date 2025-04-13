using Timetable.Frontend.BellsSchedule.Models;

namespace Timetable.Frontend.BellsSchedule.Helpers;

public static class BellTimeResolver
{
    public static BellTableRow? ResolveRowAndState(List<BellTableRow> rows, int totalMinutes, out LessonState state)
    {
        state = LessonState.LessonsEnded;

        if (rows.Count == 0)
        {
            return null;
        }

        if (totalMinutes < rows.First().StartTime.TotalMinutes)
        {
            state = LessonState.LessonsNotStarted;
            return rows.First();
        }

        if (totalMinutes >= rows.Last().EndTime.TotalMinutes)
        {
            state = LessonState.LessonsEnded;
            return rows.First();
        }

        var row = rows.FirstOrDefault(t =>
            totalMinutes >= t.StartTime.TotalMinutes && totalMinutes <= t.EndTime.TotalMinutes);

        if (row is not null)
        {
            state = LessonState.LessonIsGoing;
            return row;
        }

        for (var i = 0; i < rows.Count; i++)
        {
            if (i + 1 == rows.Count)
            {
                return null;
            }

            if (totalMinutes < rows[i].EndTime.TotalMinutes || totalMinutes > rows[i + 1].StartTime.TotalMinutes)
            {
                continue;
            }

            state = LessonState.NextLessonWillStartSoon;
            return rows[i + 1];
        }

        return null;
    }

    public static BellTableRow? ResolveRow(List<BellTableRow> rows, int totalMinutes)
    {
        return ResolveRowAndState(rows, totalMinutes, out _);
    }

    public static LessonState ResolveLessonState(List<BellTableRow> rows, int totalMinutes)
    {
        ResolveRowAndState(rows, totalMinutes, out var state);
        return state;
    }
}

public enum LessonState
{
    LessonsNotStarted,
    LessonIsGoing,
    NextLessonWillStartSoon,
    LessonsEnded
}