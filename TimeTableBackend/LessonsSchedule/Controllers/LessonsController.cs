using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimeTableBackend.LessonsSchedule.Dtos;
using TimeTableBackend.LessonsSchedule.Entities;
using TimeTableBackend.LessonsSchedule.Services;

namespace TimeTableBackend.LessonsSchedule.Controllers;

[ApiController]
[Route("api/lessons")]
public class LessonsController : ControllerBase
{
    private readonly LessonsService _lessonsService;
    private readonly IMapper _mapper;

    public LessonsController(LessonsService lessonsService, IMapper mapper)
    {
        _lessonsService = lessonsService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LessonDto>>> Get()
    {
        var lessons = await _lessonsService.GetAllAsync();

        return Ok(_mapper.Map<IEnumerable<LessonDto>>(lessons));
    }

    [HttpPost]
    public async Task<ActionResult<LessonDto>> Post(CreateLessonDto lesson)
    {
        var lessonEntity = _mapper.Map<Lesson>(lesson);

        await _lessonsService.CreateAsync(lessonEntity);

        var createdLesson = _mapper.Map<LessonDto>(lessonEntity);

        return Ok(createdLesson);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, UpdateLessonDto lesson)
    {
        var lessonEntity = await _lessonsService.GetByIdAsync(id);

        if (lessonEntity is null)
        {
            return NotFound();
        }

        _mapper.Map(lesson, lessonEntity);

        await _lessonsService.UpdateAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var lessonEntity = await _lessonsService.GetByIdAsync(id);

        if (lessonEntity is null)
        {
            return NotFound();
        }

        await _lessonsService.DeleteAsync(lessonEntity);

        return NoContent();
    }
}