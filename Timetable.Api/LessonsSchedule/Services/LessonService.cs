using Timetable.Api.LessonsSchedule.Entities;
using Timetable.Api.LessonsSchedule.Repositories;

namespace Timetable.Api.LessonsSchedule.Services;

public interface ILessonsService
{
    Task<IEnumerable<Lesson>> GetAllAsync();
    Task<Lesson?> GetByIdAsync(string id);
    Task<Lesson> CreateAsync();
    Task UpdateAsync(Lesson lesson);
    Task DeleteAsync(Lesson lesson);
}

public sealed class LessonService : ILessonsService
{
    private readonly ILessonsRepository _repository;

    public LessonService(ILessonsRepository repository)
    {
        _repository = repository;
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

    public async Task<Lesson> CreateAsync()
    {
        return await _repository.CreateAsync();
    }

    public async Task UpdateAsync(Lesson lesson)
    {
        await _repository.UpdateAsync(lesson);
    }

    public async Task DeleteAsync(Lesson lesson)
    {
        await _repository.DeleteAsync(lesson);
    }
}