using TimeTableBackend.LessonsSchedule.Entities;
using TimeTableBackend.LessonsSchedule.Repositories;
using DayOfWeek = TimeTableBackend.LessonsSchedule.Common.DayOfWeek;

namespace TimeTableBackend.LessonsSchedule.Services;

public interface ILessonTablesService
{
    Task<LessonTable> GetLessonTableByDayOfWeek(DayOfWeek dayOfWeek);
    Task UpdateLessonTable(LessonTable lessonTable);
    Task MakeLessonTableBackup(DayOfWeek dayOfWeek);
    Task<LessonTable?> RestoreLessonTableFromBackup(DayOfWeek dayOfWeek);
}

public sealed class LessonTablesService : ILessonTablesService
{
    private readonly ILessonTablesRepository _lessonTablesRepository;
    private readonly ILessonTablesBackupRepository _lessonTablesBackupRepository;
    private readonly IEventService _eventService;

    public LessonTablesService(ILessonTablesRepository lessonTablesRepository,
        ILessonTablesBackupRepository lessonTablesBackupRepository, IEventService eventService)
    {
        _lessonTablesRepository = lessonTablesRepository;
        _eventService = eventService;
        _lessonTablesBackupRepository = lessonTablesBackupRepository;
    }

    public async Task<LessonTable> GetLessonTableByDayOfWeek(DayOfWeek dayOfWeek)
    {
        return await _lessonTablesRepository.GetByDayOfWeekAsync(dayOfWeek);
    }

    public async Task UpdateLessonTable(LessonTable lessonTable)
    {
        await _lessonTablesRepository.UpdateAsync(lessonTable);
        await _eventService.NotifyAllClientsAboutUpdate();
    }

    public async Task MakeLessonTableBackup(DayOfWeek dayOfWeek)
    {
        var lessonTableToBackup = await _lessonTablesRepository.GetByDayOfWeekAsync(dayOfWeek);
        
        await _lessonTablesBackupRepository.CreateAsync(lessonTableToBackup);
    }

    public async Task<LessonTable?> RestoreLessonTableFromBackup(DayOfWeek dayOfWeek)
    {
        var backup = await _lessonTablesBackupRepository.GetByDayOfWeekAsync(dayOfWeek);

        if (backup is null)
        {
            return null;
        }
        
        await UpdateLessonTable(backup);
        
        return backup;
    }
}