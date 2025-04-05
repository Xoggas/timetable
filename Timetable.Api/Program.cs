using System.Reflection;
using MongoDB.Driver;
using Timetable.Api.BellsSchedule.Entities;
using Timetable.Api.BellsSchedule.Hubs;
using Timetable.Api.BellsSchedule.Services;
using Timetable.Api.LessonsSchedule.Repositories;
using Timetable.Api.LessonsSchedule.Services;
using Timetable.Api.Shared.Constraints;
using Timetable.Api.Shared.Hubs;
using Timetable.Api.Shared.Services;
using DayOfWeek = Timetable.Api.LessonsSchedule.Common.DayOfWeek;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<RouteOptions>(x => x.ConstraintMap.Add("mongoid", typeof(MongoIdConstraint)));

builder.Services.AddTransient<ILessonScheduleEventService, LessonScheduleEventService>();
builder.Services.AddTransient<BackgroundImageProvider>();
builder.Services.AddTransient<MongoDbService>();
builder.Services.AddTransient<IMongoDatabase>(_ =>
{
    var settings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
    
    var client = new MongoClient(settings?.ConnectionString);
    
    return client.GetDatabase(settings?.DatabaseName);
});

builder.Services.Configure<RouteOptions>(x => x.ConstraintMap.Add("dayofweek", typeof(DayOfWeekConstraint)));
builder.Services.AddTransient<ILessonsRepository, LessonsRepository>();
builder.Services.AddTransient<ILessonsService, LessonService>();
builder.Services.AddTransient<ILessonTablesRepository, LessonTablesRepository>();
builder.Services.AddTransient<ILessonTablesBackupRepository, LessonTablesBackupRepository>();
builder.Services.AddTransient<ILessonTableService, LessonTableService>();

builder.Services.AddTransient<IBellsScheduleEventService, BellsScheduleEventService>();
builder.Services.AddTransient<ISoundFileService, SoundFileService>();
builder.Services.AddTransient<IAutomaticEventService, AutomaticEventService>();
builder.Services.AddTransient<IBellTableService, BellTableService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapHub<LessonScheduleEventHub>("lessons-schedule");
app.MapHub<BellsScheduleEventHub>("bells-schedule");
app.MapControllers();
app.Run();