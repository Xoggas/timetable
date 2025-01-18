using TimeTable.Api.LessonsSchedule.Entities;
using TimeTable.Api.LessonsSchedule.Repositories;
using DayOfWeek = TimeTable.Api.LessonsSchedule.Common.DayOfWeek;

namespace TimeTable.Api.LessonsSchedule.Services;

public interface ILessonTableService
{
    Task<LessonTable> GetLessonTableByDayOfWeekAsync(DayOfWeek dayOfWeek);
    Task UpdateLessonTableAsync(LessonTable lessonTable);
    Task MakeLessonTableBackupAsync(DayOfWeek dayOfWeek);
    Task<LessonTable?> RestoreLessonTableFromBackupAsync(DayOfWeek dayOfWeek);
}

public sealed class LessonTableService : ILessonTableService
{
    private readonly ILessonTablesRepository _lessonTablesRepository;
    private readonly ILessonTablesBackupRepository _lessonTablesBackupRepository;
    private readonly IEventService _eventService;

    public LessonTableService(ILessonTablesRepository lessonTablesRepository,
        ILessonTablesBackupRepository lessonTablesBackupRepository, IEventService eventService)
    {
        _lessonTablesRepository = lessonTablesRepository;
        _eventService = eventService;
        _lessonTablesBackupRepository = lessonTablesBackupRepository;
    }

    public async Task<LessonTable> GetLessonTableByDayOfWeekAsync(DayOfWeek dayOfWeek)
    {
        return await _lessonTablesRepository.GetByDayOfWeekAsync(dayOfWeek);
    }

    public async Task UpdateLessonTableAsync(LessonTable lessonTable)
    {
        await _lessonTablesRepository.UpdateAsync(lessonTable);
        await _eventService.NotifyAllClientsAboutUpdate();
    }

    public async Task MakeLessonTableBackupAsync(DayOfWeek dayOfWeek)
    {
        var lessonTableToBackup = await _lessonTablesRepository.GetByDayOfWeekAsync(dayOfWeek);
        
        await _lessonTablesBackupRepository.CreateAsync(lessonTableToBackup);
    }

    public async Task<LessonTable?> RestoreLessonTableFromBackupAsync(DayOfWeek dayOfWeek)
    {
        var backup = await _lessonTablesBackupRepository.GetByDayOfWeekAsync(dayOfWeek);

        if (backup is null)
        {
            return null;
        }
        
        await UpdateLessonTableAsync(backup);
        
        return backup;
    }
}