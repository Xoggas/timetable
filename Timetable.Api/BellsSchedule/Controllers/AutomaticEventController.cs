using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Timetable.Api.BellsSchedule.Dtos;
using Timetable.Api.BellsSchedule.Entities;
using Timetable.Api.BellsSchedule.Services;

namespace Timetable.Api.BellsSchedule.Controllers;

[ApiController]
[Route("api/bells-schedule/automatic-event")]
public sealed class AutomaticEventController : ControllerBase
{
    private readonly IAutomaticEventService _automaticEventService;
    private readonly IMapper _mapper;

    public AutomaticEventController(IAutomaticEventService automaticEventService, IMapper mapper)
    {
        _automaticEventService = automaticEventService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AutomaticEventDto>>> GetAllEventsAsync()
    {
        var automaticEvents = await _automaticEventService.GetAllEvents();

        return Ok(_mapper.Map<IEnumerable<AutomaticEventDto>>(automaticEvents));
    }

    [HttpGet("{id:mongoid}")]
    public async Task<ActionResult<AutomaticEventDto>> GetEventByIdAsync(string id)
    {
        var automaticEvent = await _automaticEventService.GetEventById(id);

        if (automaticEvent is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<AutomaticEventDto>(automaticEvent));
    }

    [HttpPost]
    public async Task<ActionResult<AutomaticEventDto>> Post()
    {
        var automaticEvent = await _automaticEventService.CreateEvent();

        var automaticEventDto = _mapper.Map<AutomaticEventDto>(automaticEvent);

        return CreatedAtAction(nameof(GetEventByIdAsync), new { id = automaticEventDto.Id }, automaticEventDto);
    }

    [HttpPut("{id:mongoid}")]
    public async Task<ActionResult> UpdateEventAsync(string id, UpdateAutomaticEventDto dto)
    {
        if (await _automaticEventService.GetEventById(id) is null)
        {
            return NotFound();
        }

        var automaticEvent = _mapper.Map<AutomaticEvent>(dto);

        _mapper.Map(dto, automaticEvent);

        await _automaticEventService.UpdateEvent(id, automaticEvent);

        return NoContent();
    }

    [HttpDelete("{id:mongoid}")]
    public async Task<ActionResult> DeleteEventAsync(string id)
    {
        if (await _automaticEventService.GetEventById(id) is null)
        {
            return NotFound();
        }

        await _automaticEventService.DeleteEvent(id);

        return NoContent();
    }
}