using MongoDB.Driver;
using TimeTableBackend.LessonsSchedule.Entities;
using TimeTableBackend.Shared;
using DayOfWeek = TimeTableBackend.LessonsSchedule.Common.DayOfWeek;

namespace TimeTableBackend.LessonsSchedule.Repositories;

public interface ILessonTablesRepository
{
    Task<LessonTable> GetByDayOfWeekAsync(DayOfWeek dayOfWeek);
    Task UpdateAsync(LessonTable lessonTable);
}

public sealed class LessonTablesRepository : ILessonTablesRepository
{
    private readonly IMongoCollection<LessonTable> _lessonTablesCollection;

    public LessonTablesRepository(MongoDbService mongoDbService)
    {
        _lessonTablesCollection = mongoDbService.GetCollection<LessonTable>("lesson-tables");
    }

    public async Task<LessonTable> GetByDayOfWeekAsync(DayOfWeek dayOfWeek)
    {
        await CreateIfDoesntExistAsync(dayOfWeek);

        return await _lessonTablesCollection
            .Find(x => x.DayOfWeek == dayOfWeek)
            .FirstAsync();
    }

    public async Task UpdateAsync(LessonTable lessonTable)
    {
        await CreateIfDoesntExistAsync(lessonTable.DayOfWeek);
        
        var update = Builders<LessonTable>.Update
            .Set(x => x.Lessons, lessonTable.Lessons);
        
        await _lessonTablesCollection.UpdateOneAsync(x =>
            x.DayOfWeek == lessonTable.DayOfWeek, update);
    }

    private async Task CreateIfDoesntExistAsync(DayOfWeek dayOfWeek)
    {
        var table = await _lessonTablesCollection
            .Find(x => x.DayOfWeek == dayOfWeek)
            .FirstOrDefaultAsync();

        if (table is null)
        {
            await _lessonTablesCollection.InsertOneAsync(new LessonTable
            {
                DayOfWeek = dayOfWeek
            });
        }
    }
}