using Timer = System.Timers.Timer;

namespace Timetable.Frontend.Shared.Extensions;

public sealed class CustomTimer : IDisposable
{
    private readonly Timer? _timer;

    public CustomTimer(TimeSpan interval, Action callback)
    {
        _timer = new Timer(interval);
        _timer.Elapsed += (_, _) => callback();
        _timer.AutoReset = true;
        _timer.Enabled = true;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}