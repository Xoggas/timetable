using System.Reflection;
using Timetable.Api.BellsSchedule;
using Timetable.Api.BellsSchedule.Hubs;
using Timetable.Api.LessonsSchedule;
using Timetable.Api.Shared;
using Timetable.Api.Shared.Hubs;

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

builder.AddModule<SharedModule>();
builder.AddModule<LessonsScheduleModule>();
builder.AddModule<BellsScheduleModule>();

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