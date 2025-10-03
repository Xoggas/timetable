using MongoDB.Driver;
using Timetable.Api.Shared.Constraints;
using Timetable.Api.Shared.Services;

namespace Timetable.Api.Shared;

public sealed class SharedModule : IModule
{
    public void Register(WebApplicationBuilder builder)
    {
        builder.Services.Configure<RouteOptions>(x => x.ConstraintMap.Add("dayofweek", typeof(DayOfWeekConstraint)));
        builder.Services.Configure<RouteOptions>(x => x.ConstraintMap.Add("mongoid", typeof(MongoIdConstraint)));
        builder.Services.AddTransient<BackgroundImageProvider>();

        builder.Services.AddTransient<IMongoDbService, MongoDbService>();
        builder.Services.AddTransient<MongoDbService>();
        builder.Services.AddTransient<IMongoDatabase>(_ =>
        {
            var settings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
            var client = new MongoClient(settings?.ConnectionString);
            return client.GetDatabase(settings?.DatabaseName);
        });
    }
}