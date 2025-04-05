using MongoDB.Bson;

namespace Timetable.Api.Shared.Constraints;

public sealed class MongoIdConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values,
        RouteDirection routeDirection)
    {
        if (values.TryGetValue(routeKey, out var value) && value is string id)
        {
            return ObjectId.TryParse(id, out _);
        }

        return false;
    }
}