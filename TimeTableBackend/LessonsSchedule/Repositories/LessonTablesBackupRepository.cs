using MongoDB.Driver;
using TimeTableBackend.LessonsSchedule.Entities;
using TimeTableBackend.Shared;
using DayOfWeek = TimeTableBackend.LessonsSchedule.Common.DayOfWeek;

namespace TimeTableBackend.LessonsSchedule.Repositories;

public interface ILessonTablesBackupRepository
{
    Task<LessonTable?> GetByDayOfWeekAsync(DayOfWeek dayOfWeek);
    Task CreateAsync(LessonTable lessonTable);
}

public sealed class LessonTablesBackupRepository : ILessonTablesBackupRepository
{
    private readonly IMongoCollection<LessonTable> _lessonTablesBackupCollection;

    public LessonTablesBackupRepository(MongoDbService mongoDbService)
    {
        _lessonTablesBackupCollection = mongoDbService.GetCollection<LessonTable>("lesson-tables-backups");
    }

    public async Task<LessonTable?> GetByDayOfWeekAsync(DayOfWeek dayOfWeek)
    {
        return await _lessonTablesBackupCollection
            .Find(x => x.DayOfWeek == dayOfWeek)
            .FirstOrDefaultAsync();
    }

    public async Task CreateAsync(LessonTable lessonTable)
    {
        var update = Builders<LessonTable>.Update
            .Set(x => x.DayOfWeek, DayOfWeek.Monday)
            .Set(x => x.Lessons, lessonTable.Lessons);

        await _lessonTablesBackupCollection
            .UpdateOneAsync(x => x.DayOfWeek == lessonTable.DayOfWeek, update, new UpdateOptions { IsUpsert = true });
    }
}