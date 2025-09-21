using Timetable.Frontend.BellsSchedule.Models;

namespace Timetable.Frontend.BellsSchedule.Helpers;

public static class BellTimeResolver
{
    public static BellTableRow? ResolveRowAndState(List<BellTableRow> rows, int totalMinutes, out LessonState state)
    {
        state = LessonState.AfterLessons;

        if (rows.Count == 0)
        {
            return null;
        }

        if (totalMinutes < rows.First().StartTime.TotalMinutes)
        {
            state = LessonState.BeforeLessons;
            return rows.First();
        }

        if (totalMinutes >= rows.Last().EndTime.TotalMinutes)
        {
            state = LessonState.AfterLessons;
            return rows.Last();
        }

        var resolvedRow = default(BellTableRow);

        foreach (var row in rows)
        {
            if (totalMinutes >= row.StartTime.TotalMinutes && totalMinutes < row.EndTime.TotalMinutes)
            {
                resolvedRow = row;
            }

            state = LessonState.LessonIsGoing;
        }

        if (resolvedRow is not null)
        {
            return resolvedRow;
        }

        for (var i = 0; i < rows.Count; i++)
        {
            if (i + 1 == rows.Count)
            {
                break;
            }
            
            var row = rows[i];
            var nextRow = rows[i + 1];
            
            if (totalMinutes >= row.EndTime.TotalMinutes && totalMinutes < nextRow.StartTime.TotalMinutes)
            {
                resolvedRow = row;
            }
            
            state = LessonState.Break;
        }
        
        return resolvedRow;
    }
}

public enum LessonState
{
    BeforeLessons,
    LessonIsGoing,
    Break,
    AfterLessons,
    None,
}