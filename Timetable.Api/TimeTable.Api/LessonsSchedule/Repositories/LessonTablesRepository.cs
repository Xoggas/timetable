using MongoDB.Driver;
using TimeTable.Api.LessonsSchedule.Entities;
using TimeTable.Api.Shared;
using DayOfWeek = TimeTable.Api.LessonsSchedule.Common.DayOfWeek;

namespace TimeTable.Api.LessonsSchedule.Repositories;

public interface ILessonTablesRepository
{
    Task<LessonTable?> GetAsync(DayOfWeek dayOfWeek);
    Task <LessonTable> CreateAsync(DayOfWeek dayOfWeek);
    Task UpdateAsync(LessonTable lessonTable);
}

public sealed class LessonTablesRepository : ILessonTablesRepository
{
    private readonly IMongoCollection<LessonTable> _lessonTablesCollection;

    public LessonTablesRepository(MongoDbService mongoDbService)
    {
        _lessonTablesCollection = mongoDbService.GetCollection<LessonTable>("lesson-tables");
    }

    public async Task<LessonTable?> GetAsync(DayOfWeek dayOfWeek)
    {
        return await _lessonTablesCollection
            .Find(x => x.DayOfWeek == dayOfWeek)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(LessonTable lessonTable)
    {
        var update = Builders<LessonTable>.Update
            .Set(x => x.Lessons, lessonTable.Lessons);

        await _lessonTablesCollection.UpdateOneAsync(x =>
            x.DayOfWeek == lessonTable.DayOfWeek, update, new UpdateOptions
        {
            IsUpsert = true
        });
    }

    public async Task<LessonTable> CreateAsync(DayOfWeek dayOfWeek)
    {
        await _lessonTablesCollection.InsertOneAsync(new LessonTable
        {
            DayOfWeek = dayOfWeek
        });
        
        return await _lessonTablesCollection
            .Find(x => x.DayOfWeek == dayOfWeek)
            .FirstAsync();
    }
}