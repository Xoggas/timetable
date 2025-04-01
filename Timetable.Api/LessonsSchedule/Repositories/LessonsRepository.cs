﻿using MongoDB.Driver;
using Timetable.Api.LessonsSchedule.Entities;
using Timetable.Api.Shared.Services;

namespace Timetable.Api.LessonsSchedule.Repositories;

public interface ILessonsRepository
{
    Task<IEnumerable<Lesson>> GetAllAsync();
    Task<Lesson?> GetByIdAsync(string id);
    Task<Lesson> CreateAsync();
    Task UpdateAsync(Lesson lesson);
    Task DeleteAsync(Lesson lesson);
}

public sealed class LessonsRepository : ILessonsRepository
{
    private readonly IMongoCollection<Lesson> _lessonsCollection;

    public LessonsRepository(MongoDbService mongoDbService)
    {
        _lessonsCollection = mongoDbService.GetCollection<Lesson>("lessons");
    }

    public async Task<IEnumerable<Lesson>> GetAllAsync()
    {
        return await _lessonsCollection
            .Find(l => true)
            .ToListAsync();
    }

    public async Task<Lesson?> GetByIdAsync(string id)
    {
        return await _lessonsCollection
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Lesson> CreateAsync()
    {
        var lesson = new Lesson();
        
        await _lessonsCollection.InsertOneAsync(lesson);

        return lesson;
    }

    public async Task UpdateAsync(Lesson lesson)
    {
        var update = Builders<Lesson>.Update.Set(x => x.Name, lesson.Name);
        
        await _lessonsCollection.UpdateOneAsync(x => x.Id == lesson.Id, update);
    }

    public async Task DeleteAsync(Lesson lesson)
    {
        await _lessonsCollection.DeleteOneAsync(x => x.Id == lesson.Id);
    }
}