using Timetable.Api.LessonsSchedule.Repositories;
using Timetable.Api.LessonsSchedule.Services;
using Timetable.Api.Shared;
using Timetable.Api.Shared.Services;

namespace Timetable.Api.LessonsSchedule;

public sealed class LessonsScheduleModule : IModule
{
    public void Register(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ILessonScheduleEventService, LessonScheduleEventService>();
        builder.Services.AddTransient<ILessonsRepository, LessonsRepository>();
        builder.Services.AddTransient<ILessonsService, LessonService>();
        builder.Services.AddTransient<ILessonTablesRepository, LessonTablesRepository>();
        builder.Services.AddTransient<ILessonTablesBackupRepository, LessonTablesBackupRepository>();
        builder.Services.AddTransient<ILessonTableService, LessonTableService>();
    }
}