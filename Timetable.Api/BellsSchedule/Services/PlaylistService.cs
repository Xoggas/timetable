using MongoDB.Driver;
using Timetable.Api.BellsSchedule.Entities;
using Timetable.Api.LessonsSchedule.Common;
using Timetable.Api.Shared.Services;

namespace Timetable.Api.BellsSchedule.Services;

public interface IPlaylistService
{
    Task<Playlist> GetByDayOfWeekAsync(CustomDayOfWeek dayOfWeek);
    Task UpdateAsync(CustomDayOfWeek dayOfWeek, Playlist playlist);
}

public sealed class PlaylistService : IPlaylistService
{
    private readonly IMongoCollection<Playlist> _playlistCollection;

    public PlaylistService(MongoDbService mongoDbService)
    {
        _playlistCollection = mongoDbService.GetCollection<Playlist>("playlists");
    }

    public async Task<Playlist> GetByDayOfWeekAsync(CustomDayOfWeek dayOfWeek)
    {
        var playlist = await _playlistCollection.Find(x => x.DayOfWeek == dayOfWeek)
            .FirstOrDefaultAsync();

        if (playlist is not null)
        {
            return playlist;
        }

        playlist = new Playlist
        {
            DayOfWeek = dayOfWeek
        };

        await _playlistCollection.InsertOneAsync(playlist);

        return playlist;
    }

    public async Task UpdateAsync(CustomDayOfWeek dayOfWeek, Playlist playlist)
    {
        var update = Builders<Playlist>.Update
            .Set(x => x.DayOfWeek, dayOfWeek)
            .Set(x => x.SoundFilesIds, playlist.SoundFilesIds);

        await _playlistCollection.UpdateOneAsync(x => x.DayOfWeek == dayOfWeek, update,
            new UpdateOptions { IsUpsert = true });
    }
}