using Timetable.Api.BellsSchedule.Entities;

namespace Timetable.Api.BellsSchedule.BackgroundServices;

public interface IBellTableSharedEventBus
{
    event Action<BellTable>? Updated;
    void Update(BellTable bellTable);
}

public class BellTableSharedEventBus : IBellTableSharedEventBus
{
    public event Action<BellTable>? Updated;

    public void Update(BellTable bellTable)
    {
        Updated?.Invoke(bellTable);
    }
}