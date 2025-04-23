using MongoDB.Driver;
using Timetable.Api.LessonsSchedule.Common;
using Timetable.Api.LessonsSchedule.Entities;
using Timetable.Api.Shared.Services;

namespace Timetable.Api.LessonsSchedule.Repositories;

public interface ILessonTablesRepository
{
    Task<LessonTable?> GetAsync(CustomDayOfWeek customDayOfWeek);
    Task<LessonTable> CreateAsync(CustomDayOfWeek customDayOfWeek);
    Task UpdateAsync(LessonTable lessonTable);
}

public sealed class LessonTablesRepository : ILessonTablesRepository
{
    private static readonly string[][] s_emptyLessonTable =
    [
        [
            string.Empty
        ],
        [
            string.Empty
        ]
    ];

    private readonly IMongoCollection<LessonTable> _lessonTablesCollection;

    public LessonTablesRepository(MongoDbService mongoDbService)
    {
        _lessonTablesCollection = mongoDbService.GetCollection<LessonTable>("lesson-tables");
    }

    public async Task<LessonTable?> GetAsync(CustomDayOfWeek customDayOfWeek)
    {
        return await _lessonTablesCollection
            .Find(x => x.DayOfWeek == customDayOfWeek)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(LessonTable lessonTable)
    {
        var update = Builders<LessonTable>.Update
            .Set(x => x.Lessons, lessonTable.Lessons);

        await _lessonTablesCollection.UpdateOneAsync(x => x.DayOfWeek == lessonTable.DayOfWeek, update,
            new UpdateOptions { IsUpsert = true });
    }

    public async Task<LessonTable> CreateAsync(CustomDayOfWeek customDayOfWeek)
    {
        await _lessonTablesCollection.InsertOneAsync(new LessonTable
        {
            DayOfWeek = customDayOfWeek,
            Lessons = s_emptyLessonTable
        });

        return await _lessonTablesCollection
            .Find(x => x.DayOfWeek == customDayOfWeek)
            .FirstAsync();
    }
}