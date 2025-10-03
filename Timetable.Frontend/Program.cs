using Microsoft.AspNetCore.Http.Features;
using Timetable.Frontend.BellsSchedule.Services;
using Timetable.Frontend.Components;
using Timetable.Frontend.LessonsSchedule.Services;
using Timetable.Frontend.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => { options.Limits.MaxRequestBodySize = long.MaxValue; });
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.Configure<FormOptions>(x => { x.MultipartBodyLengthLimit = long.MaxValue; });

builder.Services.AddTransient<ApiUrlService>();
builder.Services.AddTransient<HttpClient>(_ =>
{
    var client = new HttpClient();

    client.BaseAddress = new Uri(builder.Configuration.GetConnectionString("ApiUrl") ??
                                 throw new ArgumentException("Api Url not configured."));

    return client;
});

builder.Services.AddTransient<LessonListService>();
builder.Services.AddTransient<LessonTableService>();

builder.Services.AddTransient<SoundFileService>();
builder.Services.AddTransient<BellTableService>();
builder.Services.AddTransient<BellTableEventService>();
builder.Services.AddTransient<AutomaticEventService>();
builder.Services.AddTransient<ManualEventService>();

var app = builder.Build();

if (app.Environment.IsDevelopment() is false)
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

var bellTableEventService = app.Services.GetRequiredService<BellTableEventService>();
await bellTableEventService.OpenConnectionAsync();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Run();