using TimeTableBackend.LessonsSchedule.Hubs;
using TimeTableBackend.LessonsSchedule.Repositories;
using TimeTableBackend.LessonsSchedule.Services;
using TimeTableBackend.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Logging.AddConsole();

builder.Services.AddControllers();

builder.Services.AddSignalR();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddTransient<IEventService, EventService>();

builder.Services.AddTransient<ILessonsRepository, LessonsRepository>();

builder.Services.AddTransient<ILessonsService, LessonsService>();

builder.Services.AddTransient<ILessonTablesRepository, LessonTablesRepository>();

builder.Services.AddTransient<ILessonTablesBackupRepository, LessonTablesBackupRepository>();

builder.Services.AddTransient<ILessonTablesService, LessonTablesService>();

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddTransient<MongoDbService>();

var app = builder.Build();

app.MapHub<EventHub>("lessons-schedule");

app.MapControllers();

app.Run();