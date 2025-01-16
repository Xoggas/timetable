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

    /// <summary>
    /// Gets all lessons.
    /// </summary>
    /// <returns>A collection of lessons.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LessonDto>>> Get()
    {
        var lessons = await _lessonsService.GetAllAsync();

        return Ok(_mapper.Map<IEnumerable<LessonDto>>(lessons));
    }

    /// <summary>
    /// Creates a new lesson.
    /// </summary>
    /// <param name="dto">A DTO for creation of a new lesson.</param>
    /// <returns>A newly created lesson.</returns>
    [HttpPost]
    public async Task<ActionResult<LessonDto>> Post(CreateLessonDto dto)
    {
        var lessonEntity = _mapper.Map<Lesson>(dto);

        await _lessonsService.CreateAsync(lessonEntity);

        var createdLesson = _mapper.Map<LessonDto>(lessonEntity);

        return Ok(createdLesson);
    }

    /// <summary>
    /// Updates an existing lesson.
    /// </summary>
    /// <param name="id">The id of an existing lesson.</param>
    /// <param name="dto">The DTO that contains updated data.</param>
    /// <returns></returns>
    /// <response code="204">If the lesson was updated.</response>
    /// <response code="404">If the lesson ID was incorrect or validation errors were present.</response>
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

    /// <summary>
    /// Deletes a lesson.
    /// </summary>
    /// <param name="id">The ID of the lesson to delete.</param>
    /// <returns></returns>
    /// <response code="204">If the lesson was deleted.</response>
    /// <response code="404">If the ID was incorrect.</response>
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