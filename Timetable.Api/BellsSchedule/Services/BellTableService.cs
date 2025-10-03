using MongoDB.Driver;
using Timetable.Api.BellsSchedule.BackgroundServices;
using Timetable.Api.BellsSchedule.Dtos;
using Timetable.Api.BellsSchedule.Entities;
using Timetable.Api.Shared.Services;

namespace Timetable.Api.BellsSchedule.Services;

public interface IBellTableService
{
    Task<BellTable> GetBellTableAsync();
    Task UpdateBellTableAsync(BellTable bellTable);
    TimeState GetTimeState(BellTable bellTable);
}

public sealed class BellTableService : IBellTableService
{
    private readonly IMongoCollection<BellTable> _bellTableCollection;
    private readonly IBellsScheduleEventService _eventService;
    private readonly BellTableSharedEventBus _bellTableSharedEventBus;

    public BellTableService(MongoDbService mongoDbService, IBellsScheduleEventService eventService,
        BellTableSharedEventBus bellTableSharedEventBus)
    {
        _bellTableCollection = mongoDbService.GetCollection<BellTable>("bell-table");
        _eventService = eventService;
        _bellTableSharedEventBus = bellTableSharedEventBus;
    }

    public async Task<BellTable> GetBellTableAsync()
    {
        var bellTable = await _bellTableCollection.Find(_ => true).FirstOrDefaultAsync();

        if (bellTable is not null)
        {
            return bellTable;
        }

        bellTable = new BellTable();

        await _bellTableCollection.InsertOneAsync(bellTable);

        return bellTable;
    }

    public async Task UpdateBellTableAsync(BellTable bellTable)
    {
        var update = Builders<BellTable>.Update
            .Set(x => x.Rows, bellTable.Rows);

        await _bellTableCollection.UpdateOneAsync(x => true, update, new UpdateOptions
        {
            IsUpsert = true
        });

        await _eventService.NotifyAllClientsAboutUpdate(bellTable);

        _bellTableSharedEventBus.Update(bellTable);
    }

    public TimeState GetTimeState(BellTable bellTable)
    {
        var rows = bellTable.Rows;
        var currentTimeInMinutes = DateTime.Now.Hour * 60 + DateTime.Now.Minute;

        if (rows.Length == 0)
        {
            return new TimeState(LessonState.None);
        }

        if (currentTimeInMinutes < rows.First().StartTime.TotalMinutes)
        {
            return new TimeState(LessonState.BeforeLessons);
        }

        if (currentTimeInMinutes >= rows.Last().EndTime.TotalMinutes)
        {
            return new TimeState(LessonState.AfterLessons, rows.Length - 1, rows.Last());
        }

        foreach (var (i, row) in rows.Index())
        {
            if (currentTimeInMinutes >= row.StartTime.TotalMinutes && currentTimeInMinutes < row.EndTime.TotalMinutes)
            {
                return new TimeState(LessonState.LessonIsGoing, i, row);
            }
        }

        for (var i = 1; i < rows.Length; i++)
        {
            var previousRow = rows[i - 1];
            var currentRow = rows[i];

            if (currentTimeInMinutes >= previousRow.EndTime.TotalMinutes &&
                currentTimeInMinutes < currentRow.StartTime.TotalMinutes)
            {
                return new TimeState(LessonState.Break, i, currentRow, i - 1, previousRow);
            }
        }

        return new TimeState(LessonState.None);
    }
}