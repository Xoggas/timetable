using Microsoft.AspNetCore.SignalR.Client;

namespace Timetable.Frontend.LessonsSchedule.Policies;

public sealed class InfiniteRetryPolicy : IRetryPolicy
{
    public TimeSpan? NextRetryDelay(RetryContext retryContext)
    {
        return TimeSpan.FromSeconds(5);
    }
}