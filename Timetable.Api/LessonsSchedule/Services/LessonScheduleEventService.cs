using Microsoft.AspNetCore.SignalR;
using Timetable.Api.LessonsSchedule.Entities;
using Timetable.Api.Shared.Hubs;

namespace Timetable.Api.Shared.Services;

public interface ILessonScheduleEventService
{
    Task NotifyAllClientsAboutUpdate(LessonTable lessonTable);
}

public sealed class LessonScheduleEventService : ILessonScheduleEventService
{
    private readonly IHubContext<LessonScheduleEventHub, ILessonScheduleEventHub> _eventHub;

    public LessonScheduleEventService(IHubContext<LessonScheduleEventHub, ILessonScheduleEventHub> eventHub)
    {
        _eventHub = eventHub;
    }

    public async Task NotifyAllClientsAboutUpdate(LessonTable lessonTable)
    {
        await _eventHub.Clients.All.Update(lessonTable);
    }
}