using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TimeTableBackend.LessonsSchedule.Models;

namespace TimeTableBackend.LessonsSchedule.Services;

public sealed class LessonListService
{
    private readonly IMongoCollection<Lesson> _lessonsCollection;

    public LessonListService(IOptions<LessonListDatabaseSettings> databaseSettings)
    {
        var client = new MongoClient(databaseSettings.Value.ConnectionString);

        var database = client.GetDatabase(databaseSettings.Value.DatabaseName);

        _lessonsCollection = database.GetCollection<Lesson>(databaseSettings.Value.CollectionName);
    }

    public async Task<List<Lesson>> GetAllAsync()
    {
        return await _lessonsCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Lesson?> GetByIdAsync(string id)
    {
        return await _lessonsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }
    
    public async Task CreateAsync(Lesson lesson)
    {
        await _lessonsCollection.InsertOneAsync(lesson);
    }

    public async Task UpdateAsync(Lesson lesson)
    {
        await _lessonsCollection.ReplaceOneAsync(x => x.Id == lesson.Id, lesson);
    }

    public async Task DeleteAsync(string id)
    {
        await _lessonsCollection.DeleteOneAsync(x => x.Id == id);
    }
}