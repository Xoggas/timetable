using Microsoft.EntityFrameworkCore;
using TimeTableBackend.LessonsSchedule.Entities;

namespace TimeTableBackend.LessonsSchedule.DbContexts;

public class LessonsScheduleDbContext : DbContext
{
    public LessonsScheduleDbContext(
        DbContextOptions<LessonsScheduleDbContext> options) : base(options)
    {
    }

    public DbSet<Lesson> Lessons { get; init; }
}