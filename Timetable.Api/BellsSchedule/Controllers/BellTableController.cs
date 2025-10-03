using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Timetable.Api.BellsSchedule.Dtos;
using Timetable.Api.BellsSchedule.Entities;
using Timetable.Api.BellsSchedule.Services;

namespace Timetable.Api.BellsSchedule.Controllers;

[ApiController]
[Route("api/bells-schedule/bell-table")]
public sealed class BellTableController : ControllerBase
{
    private readonly IBellTableService _bellTableService;
    private readonly IMapper _mapper;

    public BellTableController(IBellTableService bellTableService, IMapper mapper)
    {
        _bellTableService = bellTableService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<BellTableDto>> GetTableAsync()
    {
        var bellTable = await _bellTableService.GetBellTableAsync();

        return Ok(_mapper.Map<BellTableDto>(bellTable));
    }

    [HttpGet("time-state")]
    public async Task<ActionResult<TimeStateDto>> GetTimeStateAsync()
    {
        var bellTable = await _bellTableService.GetBellTableAsync();
        var timeState = _bellTableService.GetTimeState(bellTable);
        return Ok(_mapper.Map<TimeStateDto>(timeState));
    }

    [HttpPut]
    public async Task<ActionResult> UpdateTableAsync(UpdateBellTableDto dto)
    {
        var bellTable = _mapper.Map<BellTable>(dto);

        await _bellTableService.UpdateBellTableAsync(bellTable);

        return NoContent();
    }
}