using TimeTableBackend.LessonsSchedule.Models;
using TimeTableBackend.LessonsSchedule.Services;
using TimeTableBackend.Shared;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc();

builder.Services.Configure<LessonListDatabaseSettings>(
    builder.Configuration.GetSection(Constants.LessonsDatabaseConfigSectionName));

builder.Services.AddSingleton<LessonListService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();