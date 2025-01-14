using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimeTableBackend.LessonsSchedule.Dtos;
using TimeTableBackend.LessonsSchedule.Entities;
using TimeTableBackend.LessonsSchedule.Services;
using DayOfWeek = TimeTableBackend.LessonsSchedule.Common.DayOfWeek;

namespace TimeTableBackend.LessonsSchedule.Controllers;

[ApiController]
[Route("api/lesson-table/{dayOfWeek}")]
public class LessonTablesController : ControllerBase
{
    private readonly ILessonTablesService _lessonTablesService;
    private readonly IMapper _mapper;

    public LessonTablesController(ILessonTablesService lessonTablesService, IMapper mapper)
    {
        _lessonTablesService = lessonTablesService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<LessonTableDto>> Get(DayOfWeek dayOfWeek)
    {
        var lessonTableEntity = await _lessonTablesService.GetLessonTableByDayOfWeek(dayOfWeek);

        return Ok(_mapper.Map<LessonTableDto>(lessonTableEntity));
    }

    [HttpPut]
    public async Task<ActionResult> Put(DayOfWeek dayOfWeek, UpdateLessonTableDto dto)
    {
        var lessonTableEntity = new LessonTable
        {
            DayOfWeek = dayOfWeek,
            Lessons = dto.Lessons
        };
        
        await _lessonTablesService.UpdateLessonTable(lessonTableEntity);

        return NoContent();
    }
}