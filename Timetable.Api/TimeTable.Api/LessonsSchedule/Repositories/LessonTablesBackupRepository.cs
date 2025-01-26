using MongoDB.Driver;
using Timetable.Api.LessonsSchedule.Entities;
using Timetable.Api.Shared;
using Common_DayOfWeek = Timetable.Api.LessonsSchedule.Common.DayOfWeek;
using DayOfWeek = Timetable.Api.LessonsSchedule.Common.DayOfWeek;

namespace Timetable.Api.LessonsSchedule.Repositories;

public interface ILessonTablesBackupRepository
{
    Task<LessonTable?> GetByDayOfWeekAsync(Common_DayOfWeek dayOfWeek);
    Task CreateAsync(LessonTable lessonTable);
}

public sealed class LessonTablesBackupRepository : ILessonTablesBackupRepository
{
    private readonly IMongoCollection<LessonTable> _lessonTablesBackupCollection;

    public LessonTablesBackupRepository(MongoDbService mongoDbService)
    {
        _lessonTablesBackupCollection = mongoDbService.GetCollection<LessonTable>("lesson-tables-backups");
    }

    public async Task<LessonTable?> GetByDayOfWeekAsync(Common_DayOfWeek dayOfWeek)
    {
        return await _lessonTablesBackupCollection
            .Find(x => x.DayOfWeek == dayOfWeek)
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