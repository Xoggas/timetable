using AutoMapper;
using Timetable.Api.BellsSchedule.Dtos;
using Timetable.Api.BellsSchedule.Entities;
using Timetable.Api.BellsSchedule.Services;
using Timetable.Api.Shared.Services;

namespace Timetable.Api.BellsSchedule.BackgroundServices;

public sealed class TimeStateNotificationService : BackgroundService
{
    private readonly IBellsScheduleEventService _bellsScheduleEventService;
    private readonly IBellTableService _bellTableService;
    private readonly IMapper _mapper;

    private BellTable _bellTable = null!;
    private int _lastActivationTime = -1;
    private LessonState _lastState = LessonState.None;

    public TimeStateNotificationService
    (
        IBellsScheduleEventService bellsScheduleEventService,
        IBellTableService bellTableService,
        BellTableSharedEventBus bellTableSharedEventBus,
        IMapper mapper
    )
    {
        _bellsScheduleEventService = bellsScheduleEventService;
        _bellTableService = bellTableService;
        _mapper = mapper;
        bellTableSharedEventBus.Updated += OnBellTableUpdated;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _bellTable = await _bellTableService.GetBellTableAsync();

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (cancellationToken.IsCancellationRequested is false)
        {
            var time = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
            var timeState = _bellTableService.GetTimeState(_bellTable);

            if (time == _lastActivationTime || _lastState == timeState.LessonState)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(10), cancellationToken);
                continue;
            }

            _lastActivationTime = time;
            _lastState = timeState.LessonState;

            var timeStateDto = _mapper.Map<TimeStateDto>(timeState);
            await _bellsScheduleEventService.NotifyAllClientsAboutTimeStateChange(timeStateDto);
        }
    }

    private void OnBellTableUpdated(BellTable bellTable)
    {
        _bellTable = bellTable;
        _lastActivationTime = -1;
        _lastState = LessonState.None;
    }
}