using TimeTableBackend.LessonsSchedule.Entities;

namespace TimeTableBackend.LessonsSchedule.Services;

public sealed class LessonsService
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

    public async Task UpdateAsync()
    {
        await _repository.UpdateAsync();
        await _eventService.NotifyAllClientsAboutUpdate();
    }

    public async Task DeleteAsync(Lesson lesson)
    {
        await _repository.DeleteAsync(lesson);
        await _eventService.NotifyAllClientsAboutUpdate();
    }
}