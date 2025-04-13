using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Forms;
using Timetable.Frontend.BellsSchedule.Models;

namespace Timetable.Frontend.BellsSchedule.Services;

public sealed class SoundFileService
{
    private const int MaxFileSize = 20 * 1024 * 1024;

    private readonly HttpClient _httpClient;

    public SoundFileService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<SoundFile>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<SoundFile>>("api/bells-schedule/sound-file") ??
               throw new Exception("An error occured when retrieving the sound files");
    }

    public async Task UploadAsync(IEnumerable<IBrowserFile> files)
    {
        foreach (var file in files)
        {
            using var content = new MultipartFormDataContent();

            var streamContent = new StreamContent(file.OpenReadStream(MaxFileSize));

            streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            content.Add(streamContent, "files", file.Name);

            await _httpClient.PostAsync("api/bells-schedule/sound-file", content);
        }
    }

    public async Task UpdateAsync(IEnumerable<SoundFile> files)
    {
        foreach (var file in files)
        {
            await _httpClient.PutAsJsonAsync($"api/bells-schedule/sound-file/{file.FileId}", file);
        }
    }

    public async Task DeleteSoundFile(SoundFile soundFile)
    {
        await _httpClient.DeleteAsync($"api/bells-schedule/sound-file/{soundFile.FileId}");
    }
}