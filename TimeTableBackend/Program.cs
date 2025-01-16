using System.Reflection;
using TimeTableBackend.LessonsSchedule.Hubs;
using TimeTableBackend.LessonsSchedule.Repositories;
using TimeTableBackend.LessonsSchedule.Services;
using TimeTableBackend.Shared;

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

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddTransient<IEventService, EventService>();
builder.Services.AddTransient<ILessonsRepository, LessonsRepository>();
builder.Services.AddTransient<ILessonsService, LessonsService>();
builder.Services.AddTransient<ILessonTablesRepository, LessonTablesRepository>();
builder.Services.AddTransient<ILessonTablesBackupRepository, LessonTablesBackupRepository>();
builder.Services.AddTransient<ILessonTablesService, LessonTablesService>();
builder.Services.AddTransient<MongoDbService>();

var app = builder.Build();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<EventHub>("lessons-schedule");
app.MapControllers();
app.Run();