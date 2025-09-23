using Timetable.Api.BellsSchedule.Entities;

namespace Timetable.Api.BellsSchedule.Services;

public class BellTableUpdateSharedEventBus
{
    public event Action<BellTable>? Updated;

    public void TriggerUpdate(BellTable bellTable)
    {
        Updated?.Invoke(bellTable);
    }
}