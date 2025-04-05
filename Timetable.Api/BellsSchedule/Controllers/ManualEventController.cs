using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Timetable.Api.BellsSchedule.Dtos;
using Timetable.Api.BellsSchedule.Entities;
using Timetable.Api.BellsSchedule.Services;

namespace Timetable.Api.BellsSchedule.Controllers;

[ApiController]
[Route("api/bells-schedule/manual-event")]
public sealed class ManualEventController : ControllerBase
{
    private readonly IManualEventService _manualEventService;
    private readonly IMapper _mapper;

    public ManualEventController(IManualEventService manualEventService, IMapper mapper)
    {
        _manualEventService = manualEventService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ManualEventDto>>> Get()
    {
        var manualEvents = await _manualEventService.GetAllEvents();

        return Ok(_mapper.Map<IEnumerable<ManualEventDto>>(manualEvents));
    }

    [HttpGet("{id:mongoid}")]
    public async Task<ActionResult<ManualEventDto>> Get(string id)
    {
        var manualEvent = await _manualEventService.GetEventById(id);

        if (manualEvent is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<ManualEventDto>(manualEvent));
    }

    [HttpPost]
    public async Task<ActionResult<ManualEventDto>> Post()
    {
        var manualEvent = await _manualEventService.CreateEvent();

        var manualEventDto = _mapper.Map<ManualEventDto>(manualEvent);

        return CreatedAtAction(nameof(Get), new { id = manualEventDto.Id }, manualEventDto);
    }

    [HttpPut("{id:mongoid}")]
    public async Task<ActionResult> Put(string id, UpdateManualEventDto dto)
    {
        if (await _manualEventService.GetEventById(id) is null)
        {
            return NotFound();
        }

        var manualEvent = _mapper.Map<ManualEvent>(dto);

        _mapper.Map(dto, manualEvent);

        await _manualEventService.UpdateEvent(id, manualEvent);

        return NoContent();
    }

    [HttpDelete("{id:mongoid}")]
    public async Task<ActionResult> Delete(string id)
    {
        if (await _manualEventService.GetEventById(id) is null)
        {
            return NotFound();
        }

        await _manualEventService.DeleteEvent(id);

        return NoContent();
    }
}