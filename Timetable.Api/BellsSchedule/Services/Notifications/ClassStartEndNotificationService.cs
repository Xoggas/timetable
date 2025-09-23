using Timetable.Api.BellsSchedule.Entities;
using Timetable.Api.Shared.Services;

namespace Timetable.Api.BellsSchedule.Services;

public sealed class ClassStartEndNotificationService : BackgroundService
{
    private readonly IBellsScheduleEventService _bellsScheduleEventService;
    private readonly IBellTableService _bellTableService;
    private BellTable _bellTable = null!;
    private int _lastActivationTime = -1;

    public ClassStartEndNotificationService
    (
        IBellsScheduleEventService bellsScheduleEventService,
        IBellTableService bellTableService,
        BellTableUpdateSharedEventBus bellTableUpdateSharedEventBus
    )
    {
        _bellsScheduleEventService = bellsScheduleEventService;
        _bellTableService = bellTableService;
        bellTableUpdateSharedEventBus.Updated += OnBellTableUpdated;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _bellTable = await _bellTableService.Get();

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (cancellationToken.IsCancellationRequested is false)
        {
            var time = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
            var (state, row) = ResolveStateAndRowByCurrentTime();

            if (time == _lastActivationTime)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(10), cancellationToken);
                continue;
            }

            _lastActivationTime = time;

            if (row is null)
            {
                continue;
            }

            if (time == row.StartTime.TotalMinutes)
            {
                await _bellsScheduleEventService.NotifyAllClientsThatClassStarted();
            }
            else if (time == row.EndTime.TotalMinutes)
            {
                await _bellsScheduleEventService.NotifyAllClientsThatClassEnded(state);
            }
        }
    }

    private void OnBellTableUpdated(BellTable bellTable)
    {
        _bellTable = bellTable;
        _lastActivationTime = -1;
    }

    private (LessonState state, BellTableRow? row) ResolveStateAndRowByCurrentTime()
    {
        var rows = _bellTable.Rows;
        var totalMinutes = DateTime.Now.Hour * 60 + DateTime.Now.Minute;

        if (rows.Length == 0)
        {
            return (LessonState.None, null);
        }

        if (totalMinutes < rows.First().StartTime.TotalMinutes)
        {
            return (LessonState.BeforeLessons, rows.First());
        }

        if (totalMinutes >= rows.Last().EndTime.TotalMinutes)
        {
            return (LessonState.AfterLessons, rows.Last());
        }

        var lessonState = LessonState.AfterLessons;
        var resolvedRow = default(BellTableRow);

        foreach (var row in rows)
        {
            if (totalMinutes >= row.StartTime.TotalMinutes && totalMinutes < row.EndTime.TotalMinutes)
            {
                resolvedRow = row;
            }

            lessonState = LessonState.LessonIsGoing;
        }

        if (resolvedRow is not null)
        {
            return (lessonState, resolvedRow);
        }

        for (var i = 0; i < rows.Length; i++)
        {
            if (i + 1 == rows.Length)
            {
                break;
            }

            var row = rows[i];
            var nextRow = rows[i + 1];

            if (totalMinutes >= row.EndTime.TotalMinutes && totalMinutes < nextRow.StartTime.TotalMinutes)
            {
                resolvedRow = row;
            }

            lessonState = LessonState.Break;
        }

        return (lessonState, resolvedRow);
    }
}