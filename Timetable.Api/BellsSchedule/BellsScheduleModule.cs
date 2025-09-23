using Timetable.Api.BellsSchedule.Services;
using Timetable.Api.Shared;
using Timetable.Api.Shared.Services;

namespace Timetable.Api.BellsSchedule;

public sealed class BellsScheduleModule : IModule
{
    public void Register(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IBellsScheduleEventService, BellsScheduleEventService>();
        builder.Services.AddTransient<ISoundFileService, SoundFileService>();
        builder.Services.AddTransient<IAutomaticEventService, AutomaticEventService>();
        builder.Services.AddTransient<IManualEventService, ManualEventService>();
        builder.Services.AddTransient<IBellTableService, BellTableService>();
        builder.Services.AddSingleton<BellTableUpdateSharedEventBus>();
        builder.Services.AddHostedService<ClassStartEndNotificationService>();
    }
}