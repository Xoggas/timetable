﻿using TimeTableBackend.LessonsSchedule.Entities;
using TimeTableBackend.LessonsSchedule.Repositories;

namespace TimeTableBackend.LessonsSchedule.Services;

public interface ILessonsService
{
    Task<IEnumerable<Lesson>> GetAllAsync();
    Task<Lesson?> GetByIdAsync(int id);
    Task CreateAsync(Lesson lesson);
    Task DeleteAsync(Lesson lesson);
    Task SaveChangesAsync();
}

public sealed class LessonsService : ILessonsService
{
    private readonly LessonsRepository _repository;
    private readonly EventService _eventService;

    public LessonsService(LessonsRepository repository, EventService eventService)
    {
        _repository = repository;
        _eventService = eventService;
    }

    public async Task<IEnumerable<Lesson>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Lesson?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreateAsync(Lesson lesson)
    {
        await _repository.CreateAsync(lesson);
        await _eventService.NotifyAllClientsAboutUpdate();
    }

    public async Task DeleteAsync(Lesson lesson)
    {
        await _repository.DeleteAsync(lesson);
        await _eventService.NotifyAllClientsAboutUpdate();
    }

    public async Task SaveChangesAsync()
    {
        await _repository.SaveChangesAsync();
        await _eventService.NotifyAllClientsAboutUpdate();
    }
}