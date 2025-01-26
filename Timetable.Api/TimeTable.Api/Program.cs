using System.Reflection;
using MongoDB.Driver;
using Timetable.Api.LessonsSchedule.Hubs;
using Timetable.Api.LessonsSchedule.Repositories;
using Timetable.Api.LessonsSchedule.Services;
using Timetable.Api.Shared;

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

builder.Services.AddTransient<IEventService, EventService>();

builder.Services.AddTransient<ILessonsRepository, LessonsRepository>();
builder.Services.AddTransient<ILessonsService, LessonService>();

builder.Services.AddTransient<ILessonTablesRepository, LessonTablesRepository>();
builder.Services.AddTransient<ILessonTablesBackupRepository, LessonTablesBackupRepository>();
builder.Services.AddTransient<ILessonTableService, LessonTableService>();

builder.Services.AddTransient<MongoDbService>();
builder.Services.AddTransient<IMongoDatabase>(_ =>
{
    var settings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
    var client = new MongoClient(settings?.ConnectionString);
    return client.GetDatabase(settings?.DatabaseName);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHub<EventHub>("lessons-schedule");
app.MapControllers();

app.Run();