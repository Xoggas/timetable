using MongoDB.Driver;
using Timetable.Api.LessonsSchedule.Common;
using Timetable.Api.LessonsSchedule.Entities;
using Timetable.Api.Shared.Services;

namespace Timetable.Api.LessonsSchedule.Repositories;

public interface ILessonTablesBackupRepository
{
    Task<LessonTable?> GetByDayOfWeekAsync(CustomDayOfWeek customDayOfWeek);
    Task CreateAsync(LessonTable lessonTable);
}

public sealed class LessonTablesBackupRepository : ILessonTablesBackupRepository
{
    private readonly IMongoCollection<LessonTable> _lessonTablesBackupCollection;

    public LessonTablesBackupRepository(MongoDbService mongoDbService)
    {
        _lessonTablesBackupCollection = mongoDbService.GetCollection<LessonTable>("lesson-tables-backups");
    }

    public async Task<LessonTable?> GetByDayOfWeekAsync(CustomDayOfWeek customDayOfWeek)
    {
        return await _lessonTablesBackupCollection
            .Find(x => x.DayOfWeek == customDayOfWeek)
            .FirstOrDefaultAsync();
    }

    public async Task CreateAsync(LessonTable lessonTable)
    {
        var update = Builders<LessonTable>.Update
            .Set(x => x.DayOfWeek, lessonTable.DayOfWeek)
            .Set(x => x.Lessons, lessonTable.Lessons);

        await _lessonTablesBackupCollection.UpdateOneAsync(x => x.DayOfWeek == lessonTable.DayOfWeek, update,
            new UpdateOptions
            {
                IsUpsert = true
            });
    }
}