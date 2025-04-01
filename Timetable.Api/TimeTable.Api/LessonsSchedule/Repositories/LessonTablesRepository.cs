using MongoDB.Driver;
using Timetable.Api.LessonsSchedule.Entities;
using Timetable.Api.Shared.Services;
using Common_DayOfWeek = Timetable.Api.LessonsSchedule.Common.DayOfWeek;
using DayOfWeek = Timetable.Api.LessonsSchedule.Common.DayOfWeek;

namespace Timetable.Api.LessonsSchedule.Repositories;

public interface ILessonTablesRepository
{
    Task<LessonTable?> GetAsync(Common_DayOfWeek dayOfWeek);
    Task<LessonTable> CreateAsync(Common_DayOfWeek dayOfWeek);
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

    public async Task<LessonTable?> GetAsync(Common_DayOfWeek dayOfWeek)
    {
        return await _lessonTablesCollection
            .Find(x => x.DayOfWeek == dayOfWeek)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(LessonTable lessonTable)
    {
        var update = Builders<LessonTable>.Update
            .Set(x => x.Lessons, lessonTable.Lessons);

        await _lessonTablesCollection.UpdateOneAsync(x => x.DayOfWeek == lessonTable.DayOfWeek, update,
            new UpdateOptions { IsUpsert = true });
    }

    public async Task<LessonTable> CreateAsync(Common_DayOfWeek dayOfWeek)
    {
        await _lessonTablesCollection.InsertOneAsync(new LessonTable
        {
            DayOfWeek = dayOfWeek,
            Lessons = s_emptyLessonTable
        });

        return await _lessonTablesCollection
            .Find(x => x.DayOfWeek == dayOfWeek)
            .FirstAsync();
    }
}