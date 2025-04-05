using MongoDB.Driver;
using Timetable.Api.BellsSchedule.Entities;
using Timetable.Api.Shared.Services;

namespace Timetable.Api.BellsSchedule.Services;

public interface ISoundFileService
{
    Task<IEnumerable<SoundFile>> GetAllFileEntries();
    Task<SoundFile> Upload(IFormFile file);
    Task<byte[]> GetBytes(string id);
    Task Update(string id, SoundFile soundFile);
    Task<bool> Exists(string id);
    Task Delete(string id);
}

public sealed class SoundFileService : ISoundFileService
{
    private const string SoundFilesPath = "SoundFiles";

    private readonly IMongoCollection<SoundFile> _soundFilesCollection;

    public SoundFileService(MongoDbService mongoDbService)
    {
        _soundFilesCollection = mongoDbService.GetCollection<SoundFile>("sound-files");
    }

    public async Task<IEnumerable<SoundFile>> GetAllFileEntries()
    {
        return await _soundFilesCollection
            .Find(_ => true)
            .ToListAsync();
    }

    public async Task<SoundFile> Upload(IFormFile file)
    {
        var soundFile = new SoundFile
        {
            FileName = Path.GetFileNameWithoutExtension(file.FileName)
        };

        await _soundFilesCollection.InsertOneAsync(soundFile);

        if (Path.Exists(SoundFilesPath) is false)
        {
            Directory.CreateDirectory(SoundFilesPath);
        }

        await using var stream = File.Create(Path.Combine(SoundFilesPath, soundFile.FileId));

        await file.CopyToAsync(stream);

        return soundFile;
    }

    public async Task<byte[]> GetBytes(string id)
    {
        var path = Path.Combine(SoundFilesPath, id);

        return await File.ReadAllBytesAsync(path);
    }

    public async Task Update(string id, SoundFile soundFile)
    {
        var update = Builders<SoundFile>.Update
            .Set(s => s.FileName, soundFile.FileName);

        await _soundFilesCollection.UpdateOneAsync(x => x.FileId == id, update);
    }

    public async Task<bool> Exists(string id)
    {
        return await _soundFilesCollection.Find(x => x.FileId == id).AnyAsync();
    }

    public async Task Delete(string id)
    {
        if (await Exists(id) is false)
        {
            return;
        }

        await _soundFilesCollection.DeleteOneAsync(x => x.FileId == id);

        var path = Path.Combine(SoundFilesPath, id);

        File.Delete(path);
    }
}