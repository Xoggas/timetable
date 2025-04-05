using System.Collections;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Timetable.Api.BellsSchedule.Dtos;
using Timetable.Api.BellsSchedule.Entities;
using Timetable.Api.BellsSchedule.Services;

namespace Timetable.Api.BellsSchedule.Controllers;

[ApiController]
[Route("api/bells-schedule/sound")]
public class SoundFileController : ControllerBase
{
    private readonly ISoundFileService _soundFileService;
    private readonly IMapper _mapper;

    public SoundFileController(ISoundFileService soundFileService, IMapper mapper)
    {
        _soundFileService = soundFileService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<SoundFileDto>> Get()
    {
        var soundFiles = await _soundFileService.GetAllFileEntries();

        return Ok(_mapper.Map<IEnumerable<SoundFileDto>>(soundFiles));
    }

    [HttpGet("{id:mongoid}")]
    public async Task<ActionResult> Get(string id)
    {
        if (await _soundFileService.Exists(id) is false)
        {
            return NotFound();
        }

        var bytes = await _soundFileService.GetBytes(id);

        return File(bytes, "audio/mpeg");
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<SoundFileDto>>> Post(IFormFile[] files)
    {
        var soundFiles = new List<SoundFile>();

        if (files.Any(x => x.ContentType is not "audio/mpeg"))
        {
            return BadRequest();
        }

        foreach (var file in files)
        {
            soundFiles.Add(await _soundFileService.Upload(file));
        }

        return Ok(_mapper.Map<IEnumerable<SoundFileDto>>(soundFiles));
    }

    [HttpPut("{id:mongoid}")]
    public async Task<ActionResult> Put(string id, UpdateSoundFileDto dto)
    {
        if (await _soundFileService.Exists(id) is false)
        {
            return NotFound();
        }

        var soundFile = _mapper.Map<SoundFile>(dto);

        await _soundFileService.Update(id, soundFile);

        return NoContent();
    }

    [HttpDelete("{id:mongoid}")]
    public async Task<ActionResult> Delete(string id)
    {
        if (await _soundFileService.Exists(id) is false)
        {
            return NotFound();
        }

        await _soundFileService.Delete(id);

        return NoContent();
    }
}