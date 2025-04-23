using System.Text;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using Timetable.Api.LessonsSchedule.Common;
using Timetable.Api.LessonsSchedule.Entities;
using Timetable.Api.LessonsSchedule.Repositories;
using Timetable.Api.Shared.Services;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace Timetable.Api.LessonsSchedule.Services;

public interface ILessonTableService
{
    Task<LessonTable> GetLessonTableByDayOfWeekAsync(CustomDayOfWeek customDayOfWeek);
    Task UpdateLessonTableAsync(LessonTable lessonTable);
    Task MakeLessonTableBackupAsync(CustomDayOfWeek customDayOfWeek);
    Task<LessonTable?> RestoreLessonTableFromBackupAsync(CustomDayOfWeek customDayOfWeek);
    Task<byte[]> GetCsvLessonTableByDayOfWeekAsync(CustomDayOfWeek customDayOfWeek);
    Task<byte[]> GetJsonLessonTableByDayOfWeekAsync(CustomDayOfWeek customDayOfWeek);
}

public sealed class LessonTableService : ILessonTableService
{
    private readonly ILessonTablesRepository _lessonTablesRepository;
    private readonly ILessonTablesBackupRepository _lessonTablesBackupRepository;
    private readonly ILessonScheduleEventService _lessonScheduleEventService;

    public LessonTableService(ILessonTablesRepository lessonTablesRepository,
        ILessonTablesBackupRepository lessonTablesBackupRepository,
        ILessonScheduleEventService lessonScheduleEventService)
    {
        _lessonTablesRepository = lessonTablesRepository;
        _lessonScheduleEventService = lessonScheduleEventService;
        _lessonTablesBackupRepository = lessonTablesBackupRepository;
    }

    public async Task<LessonTable> GetLessonTableByDayOfWeekAsync(CustomDayOfWeek customDayOfWeek)
    {
        var lessonTable = await _lessonTablesRepository.GetAsync(customDayOfWeek);

        if (lessonTable is null)
        {
            return await _lessonTablesRepository.CreateAsync(customDayOfWeek);
        }

        return lessonTable;
    }

    public async Task<byte[]> GetCsvLessonTableByDayOfWeekAsync(CustomDayOfWeek customDayOfWeek)
    {
        var lessonTable = await GetLessonTableByDayOfWeekAsync(customDayOfWeek);
        var csv = string.Join(Environment.NewLine, lessonTable.Lessons.Select(x => string.Join(",", x)));
        var bytes = Encoding.UTF8.GetBytes(csv);
        return bytes;
    }

    public async Task<byte[]> GetJsonLessonTableByDayOfWeekAsync(CustomDayOfWeek customDayOfWeek)
    {
        var lessonTable = await GetLessonTableByDayOfWeekAsync(customDayOfWeek);
        var json = JsonConvert.SerializeObject(lessonTable, Formatting.Indented);
        var bytes = Encoding.UTF8.GetBytes(json);
        return bytes;
    }

    public async Task UpdateLessonTableAsync(LessonTable lessonTable)
    {
        await _lessonTablesRepository.UpdateAsync(lessonTable);
        await _lessonScheduleEventService.NotifyAllClientsAboutUpdate(lessonTable);
    }

    public async Task MakeLessonTableBackupAsync(CustomDayOfWeek customDayOfWeek)
    {
        var lessonTableToBackup = await GetLessonTableByDayOfWeekAsync(customDayOfWeek);

        await _lessonTablesBackupRepository.CreateAsync(lessonTableToBackup);
    }

    public async Task<LessonTable?> RestoreLessonTableFromBackupAsync(CustomDayOfWeek customDayOfWeek)
    {
        var backup = await _lessonTablesBackupRepository.GetByDayOfWeekAsync(customDayOfWeek);

        if (backup is null)
        {
            return null;
        }

        await UpdateLessonTableAsync(backup);

        return backup;
    }
}