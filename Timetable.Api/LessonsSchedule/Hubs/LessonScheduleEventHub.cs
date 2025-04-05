using Microsoft.AspNetCore.SignalR;
using Timetable.Api.LessonsSchedule.Entities;

namespace Timetable.Api.Shared.Hubs;

public interface ILessonScheduleEventHub
{
    public Task Update(LessonTable lessonTable);
}

public sealed class LessonScheduleEventHub : Hub<ILessonScheduleEventHub>
{
    public async Task Update(LessonTable lessonTable)
    {
        await Clients.All.Update(lessonTable);
    }
}