using Timetable.Frontend.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

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