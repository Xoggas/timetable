using Timetable.Api.LessonsSchedule.Entities;
using Timetable.Api.LessonsSchedule.Repositories;
using Timetable.Api.Shared.Services;
using Common_DayOfWeek = Timetable.Api.LessonsSchedule.Common.DayOfWeek;

namespace Timetable.Api.LessonsSchedule.Services;

public interface ILessonTableService
{
    Task<LessonTable> GetLessonTableByDayOfWeekAsync(Common_DayOfWeek dayOfWeek);
    Task UpdateLessonTableAsync(LessonTable lessonTable);
    Task MakeLessonTableBackupAsync(Common_DayOfWeek dayOfWeek);
    Task<LessonTable?> RestoreLessonTableFromBackupAsync(Common_DayOfWeek dayOfWeek);
}

public sealed class LessonTableService : ILessonTableService
{
    private readonly ILessonTablesRepository _lessonTablesRepository;
    private readonly ILessonTablesBackupRepository _lessonTablesBackupRepository;
    private readonly ILessonScheduleEventService _lessonScheduleEventService;

    public LessonTableService(ILessonTablesRepository lessonTablesRepository,
        ILessonTablesBackupRepository lessonTablesBackupRepository, ILessonScheduleEventService lessonScheduleEventService)
    {
        _lessonTablesRepository = lessonTablesRepository;
        _lessonScheduleEventService = lessonScheduleEventService;
        _lessonTablesBackupRepository = lessonTablesBackupRepository;
    }

    public async Task<LessonTable> GetLessonTableByDayOfWeekAsync(Common_DayOfWeek dayOfWeek)
    {
        var lessonTable = await _lessonTablesRepository.GetAsync(dayOfWeek);

        if (lessonTable is null)
        {
            return await _lessonTablesRepository.CreateAsync(dayOfWeek);
        }

        return lessonTable;
    }

    public async Task UpdateLessonTableAsync(LessonTable lessonTable)
    {
        await _lessonTablesRepository.UpdateAsync(lessonTable);
        await _lessonScheduleEventService.NotifyAllClientsAboutUpdate(lessonTable);
    }

    public async Task MakeLessonTableBackupAsync(Common_DayOfWeek dayOfWeek)
    {
        var lessonTableToBackup = await GetLessonTableByDayOfWeekAsync(dayOfWeek);

        await _lessonTablesBackupRepository.CreateAsync(lessonTableToBackup);
    }

    public async Task<LessonTable?> RestoreLessonTableFromBackupAsync(Common_DayOfWeek dayOfWeek)
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