using Microsoft.EntityFrameworkCore;
using TimeTableBackend.LessonsSchedule.Entities;

namespace TimeTableBackend.LessonsSchedule.Data;

public class LessonsScheduleDbContext : DbContext
{
    public LessonsScheduleDbContext(DbContextOptions<LessonsScheduleDbContext> options) : base(options)
    {
    }

    public DbSet<Lesson> Lessons { get; init; }
}