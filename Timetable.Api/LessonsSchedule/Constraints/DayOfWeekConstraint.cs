namespace Timetable.Api.Shared.Constraints;

public sealed class DayOfWeekConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values,
        RouteDirection routeDirection)
    {
        if (values.TryGetValue(routeKey, out var value) && value is string dayOfWeek)
        {
            return Enum.TryParse(dayOfWeek, out DayOfWeek _);
        }

        return false;
    }
}