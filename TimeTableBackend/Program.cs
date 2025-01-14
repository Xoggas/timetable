using Microsoft.EntityFrameworkCore;
using TimeTableBackend.LessonsSchedule.Data;
using TimeTableBackend.LessonsSchedule.Hubs;
using TimeTableBackend.LessonsSchedule.Repositories;
using TimeTableBackend.LessonsSchedule.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Logging.AddConsole();

builder.Services.AddDbContext<LessonsScheduleDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration
        .GetConnectionString("DbConnectionString"));
});

builder.Services.AddControllers();

builder.Services.AddSignalR();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddTransient<EventService>();

builder.Services.AddTransient<LessonsRepository>();

builder.Services.AddTransient<ILessonsService, LessonsService>();

var app = builder.Build();

app.MapHub<EventHub>("lessons-schedule");

app.MapControllers();

app.Run();