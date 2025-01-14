using MongoDB.Driver;
using TimeTableBackend.LessonsSchedule.Entities;
using TimeTableBackend.Shared;
using DayOfWeek = TimeTableBackend.LessonsSchedule.Common.DayOfWeek;

namespace TimeTableBackend.LessonsSchedule.Services;

public interface ILessonTablesService
{
    Task<LessonTable> GetLessonTableByDayOfWeek(DayOfWeek dayOfWeek);
    Task UpdateLessonTable(LessonTable lessonTable);
}

public sealed class LessonTablesService : ILessonTablesService
{
    private readonly IMongoCollection<LessonTable> _lessonTablesCollection;

    public LessonTablesService(MongoDbService mongoDbService)
    {
        _lessonTablesCollection = mongoDbService.GetCollection<LessonTable>("lesson-tables");
    }

    public async Task<LessonTable> GetLessonTableByDayOfWeek(DayOfWeek dayOfWeek)
    {
        await CreateTableIfDoesntExist(dayOfWeek);

        return await _lessonTablesCollection
            .Find(x => x.DayOfWeek == dayOfWeek)
            .FirstAsync();
    }

    public async Task UpdateLessonTable(LessonTable lessonTable)
    {
        await CreateTableIfDoesntExist(lessonTable.DayOfWeek);
        
        var update = Builders<LessonTable>.Update
            .Set(x => x.Lessons, lessonTable.Lessons);
        
        await _lessonTablesCollection.UpdateOneAsync(x =>
            x.DayOfWeek == lessonTable.DayOfWeek, update);
    }

    private async Task CreateTableIfDoesntExist(DayOfWeek dayOfWeek)
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