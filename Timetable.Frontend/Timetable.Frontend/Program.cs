using Timetable.Frontend.Components;
using Timetable.Frontend.LessonsSchedule.Services;
using Timetable.Frontend.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddTransient<ApiUrlService>();
builder.Services.AddTransient<LessonListService>();
builder.Services.AddTransient<LessonTableService>();
builder.Services.AddTransient<HttpClient>(_ =>
{
    var client = new HttpClient();
    
    client.BaseAddress = new Uri(builder.Configuration.GetConnectionString("ApiUrl") ??
                                 throw new ArgumentException("Api Url not configured."));
    
    return client;
});

var app = builder.Build();

if (app.Environment.IsDevelopment() is false)
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Run();