using Timetable.Api.LessonsSchedule.Entities;
using Timetable.Api.LessonsSchedule.Repositories;

namespace Timetable.Api.LessonsSchedule.Services;

public interface ILessonsService
{
    Task<IEnumerable<Lesson>> GetAllAsync();
    Task<Lesson?> GetByIdAsync(string id);
    Task CreateAsync(Lesson lesson);
    Task UpdateAsync(Lesson lesson);
    Task DeleteAsync(Lesson lesson);
}

public sealed class LessonService : ILessonsService
{
    private readonly ILessonsRepository _repository;
    private readonly IEventService _eventService;

    public LessonService(ILessonsRepository repository, IEventService eventService)
    {
        _repository = repository;
        _eventService = eventService;
    }

    public async Task<IEnumerable<Lesson>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Lesson?> GetByIdAsync(string id)
    {
        try
        {
            return await _repository.GetByIdAsync(id);
        }
        catch
        {
            return null;
        }
    }

    public async Task CreateAsync(Lesson lesson)
    {
        await _repository.CreateAsync(lesson);
        await _eventService.NotifyAllClientsAboutUpdate();
    }

    public async Task UpdateAsync(Lesson lesson)
    {
        await _repository.UpdateAsync(lesson);
        await _eventService.NotifyAllClientsAboutUpdate();
    }

    public async Task DeleteAsync(Lesson lesson)
    {
        await _repository.DeleteAsync(lesson);
        await _eventService.NotifyAllClientsAboutUpdate();
    }
}