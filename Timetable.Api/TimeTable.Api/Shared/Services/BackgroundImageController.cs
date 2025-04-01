using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using TimeTableBackend.Shared.Controllers;

namespace Timetable.Api.Shared.Services;

[ApiController]
[Route("api/random-background-image")]
[DisplayName("Lesson Controller")]
[Produces("image/jpeg")]
public sealed class BackgroundImageController : ControllerBase
{
    private readonly BackgroundImageProvider _backgroundImageProvider;

    public BackgroundImageController(BackgroundImageProvider backgroundImageProvider)
    {
        _backgroundImageProvider = backgroundImageProvider;
    }

    [HttpGet]
    public async Task<FileResult> Get()
    {
        var bytes = await _backgroundImageProvider.GetRandomBackgroundImageBytes();
        return File(bytes, "image/jpeg");
    }
}