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
    public async Task<ActionResult<BellTableDto>> Get()
    {
        var bellTable = await _bellTableService.Get();

        return Ok(_mapper.Map<BellTableDto>(bellTable));
    }

    [HttpPut]
    public async Task<ActionResult> Put(UpdateBellTableDto dto)
    {
        var bellTable = _mapper.Map<BellTable>(dto);

        await _bellTableService.Update(bellTable);

        return NoContent();
    }
}