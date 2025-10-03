using Timetable.Api.BellsSchedule.Entities;

namespace Timetable.Api.BellsSchedule.BackgroundServices;

public class BellTableSharedEventBus
{
    public event Action<BellTable>? Updated;

    public void Update(BellTable bellTable)
    {
        Updated?.Invoke(bellTable);
    }
}