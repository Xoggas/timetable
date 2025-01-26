using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;

namespace Timetable.Api.Tests.Integration.Shared;

internal sealed class TimeTableWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly IMongoDatabase _database;

    public TimeTableWebApplicationFactory(IMongoDatabase database)
    {
        _database = database;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IMongoDatabase>();
            services.AddTransient<IMongoDatabase>(_ => _database);
        });
    }
}