using Microsoft.EntityFrameworkCore;
using TimeTableBackend.LessonsSchedule.DbContexts;
using TimeTableBackend.LessonsSchedule.Entities;

namespace TimeTableBackend.LessonsSchedule.Services;

public sealed class LessonsService
{
    private readonly LessonsScheduleDbContext _dbContext;

    public LessonsService(LessonsScheduleDbContext dbContext)
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
    }

    public void Delete(Lesson lesson)
    {
        _dbContext.Lessons.Remove(lesson);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

  
}