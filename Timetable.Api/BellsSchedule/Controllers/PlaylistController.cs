using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Timetable.Api.BellsSchedule.Dtos;
using Timetable.Api.BellsSchedule.Entities;
using Timetable.Api.BellsSchedule.Services;
using Timetable.Api.LessonsSchedule.Common;

namespace Timetable.Api.BellsSchedule.Controllers;

[ApiController]
[Route("api/bells-schedule/playlist/{dayOfWeek:dayofweek}")]
public sealed class PlaylistController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPlaylistService _playlistService;

    public PlaylistController(IPlaylistService playlistService, IMapper mapper)
    {
        _playlistService = playlistService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<PlaylistDto>> Get(CustomDayOfWeek dayOfWeek)
    {
        var playlist = await _playlistService.GetByDayOfWeekAsync(dayOfWeek);

        return Ok(_mapper.Map<PlaylistDto>(playlist));
    }

    [HttpPut]
    public async Task<ActionResult> Put(CustomDayOfWeek dayOfWeek, UpdatePlaylistDto dto)
    {
        var playlist = _mapper.Map<Playlist>(dto);

        await _playlistService.UpdateAsync(dayOfWeek, playlist);

        return NoContent();
    }
}