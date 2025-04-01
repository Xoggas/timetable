using System.ComponentModel;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Timetable.Api.LessonsSchedule.Dtos;
using Timetable.Api.LessonsSchedule.Services;

namespace Timetable.Api.LessonsSchedule.Controllers;

[ApiController]
[Route("api/lesson")]
[DisplayName("Lesson Controller")]
[Produces("application/json")]
public class LessonController : ControllerBase
{
    private readonly ILessonsService _lessonsService;
    private readonly IMapper _mapper;

    public LessonController(ILessonsService lessonsService, IMapper mapper)
    {
        _lessonsService = lessonsService;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all lessons.
    /// </summary>
    /// <returns>A list of lessons.</returns>
    /// <response code="200">Returns the list of lessons.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LessonDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LessonDto>>> Get()
    {
        var lessons = await _lessonsService.GetAllAsync();

        return Ok(_mapper.Map<IEnumerable<LessonDto>>(lessons));
    }
    
    /// <summary>
    /// Retrieves a lesson by id.
    /// </summary>
    /// <returns>Found lesson.</returns>
    /// <response code="200">Returns the lesson by id.</response>
    /// <response code="404">Lesson not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(IEnumerable<LessonDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LessonDto>>> Get(string id)
    {
        var lesson = await _lessonsService.GetByIdAsync(id);

        if (lesson is null)
        {
            return NotFound();
        }
        
        return Ok(_mapper.Map<IEnumerable<LessonDto>>(lesson));
    }

    /// <summary>
    /// Creates a new lesson.
    /// </summary>
    /// <returns>The created lesson.</returns>
    /// <response code="200">Returns the created lesson.</response>
    [HttpPost]
    [ProducesResponseType(typeof(LessonDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<LessonDto>> Post()
    {
        var createdLesson = await _lessonsService.CreateAsync();

        var createdLessonDto = _mapper.Map<LessonDto>(createdLesson);

        return CreatedAtAction(nameof(Get), new { id = createdLessonDto.Id }, createdLessonDto);
    }

    /// <summary>
    /// Updates an existing lesson by ID.
    /// </summary>
    /// <param name="id">The ID of the lesson to update.</param>
    /// <param name="dto">The updated lesson data.</param>
    /// <response code="204">The lesson was successfully updated.</response>
    /// <response code="400">Validation error.</response>
    /// <response code="404">Lesson not found.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    /// Deletes a lesson by ID.
    /// </summary>
    /// <param name="id">The ID of the lesson to delete.</param>
    /// <response code="204">The lesson was successfully deleted.</response>
    /// <response code="404">Lesson not found.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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