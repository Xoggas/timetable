using TimeTableBackend.LessonsSchedule.Entities;
using TimeTableBackend.LessonsSchedule.Repositories;

namespace TimeTableBackend.LessonsSchedule.Services;

public interface ILessonsService
{
    Task<IEnumerable<Lesson>> GetAllAsync();
    Task<Lesson?> GetByIdAsync(string id);
    Task CreateAsync(Lesson lesson);
    Task UpdateAsync(Lesson lesson);
    Task DeleteAsync(Lesson lesson);
}

public sealed class LessonsService : ILessonsService
{
    private readonly ILessonsRepository _repository;
    private readonly IEventService _eventService;

    public LessonsService(ILessonsRepository repository, IEventService eventService)
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
        return await _repository.GetByIdAsync(id);
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