using Microsoft.EntityFrameworkCore;
using TimeTableBackend.LessonsSchedule.Data;
using TimeTableBackend.LessonsSchedule.Entities;

namespace TimeTableBackend.LessonsSchedule.Repositories;

public interface ILessonsRepository
{
    Task<IEnumerable<Lesson>> GetAllAsync();
    Task<Lesson?> GetByIdAsync(int id);
    Task CreateAsync(Lesson lesson);
    Task DeleteAsync(Lesson lesson);
    Task SaveChangesAsync();
}

public sealed class LessonsRepository : ILessonsRepository
{
    private readonly LessonsScheduleDbContext _dbContext;

    public LessonsRepository(LessonsScheduleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Lesson>> GetAllAsync()
    {
        return await _dbContext.Lessons.ToListAsync();
    }

    public async Task<Lesson?> GetByIdAsync(int id)
    {
        return await _dbContext.Lessons.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task CreateAsync(Lesson lesson)
    {
        await _dbContext.Lessons.AddAsync(lesson);

        await SaveChangesAsync();
    }

    public async Task DeleteAsync(Lesson lesson)
    {
        _dbContext.Lessons.Remove(lesson);

        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}