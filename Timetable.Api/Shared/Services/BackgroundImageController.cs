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

    /// <summary>
    /// Retrieves a random background image depending on the current month or event.
    /// </summary>
    /// <returns>A random background image depending on the current month or event.</returns>
    /// <response code="200">Successfully sent an image.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<FileResult> Get()
    {
        var bytes = await _backgroundImageProvider.GetRandomBackgroundImageBytes();
        
        return File(bytes, "image/jpeg");
    }
}