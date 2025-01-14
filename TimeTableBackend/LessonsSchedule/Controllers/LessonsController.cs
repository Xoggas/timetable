using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimeTableBackend.LessonsSchedule.Dtos;
using TimeTableBackend.LessonsSchedule.Entities;
using TimeTableBackend.LessonsSchedule.Services;

namespace TimeTableBackend.LessonsSchedule.Controllers;

[ApiController]
[Route("api/lesson")]
public class LessonsController : ControllerBase
{
    private readonly ILessonsService _lessonsService;
    private readonly IMapper _mapper;

    public LessonsController(ILessonsService lessonsService, IMapper mapper)
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
    public async Task<ActionResult<LessonDto>> Post(CreateLessonDto dto)
    {
        var lessonEntity = _mapper.Map<Lesson>(dto);

        await _lessonsService.CreateAsync(lessonEntity);

        var createdLesson = _mapper.Map<LessonDto>(lessonEntity);

        return Ok(createdLesson);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(string id, UpdateLessonDto dto)
    {
        var lessonEntity = await _lessonsService.GetByIdAsync(id);

        if (lessonEntity is null)
        {
            return NotFound();
        }

        _mapper.Map(dto, lessonEntity);

        await _lessonsService.UpdateAsync(lessonEntity);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
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