using Microsoft.AspNetCore.Mvc;
using TimeTableBackend.LessonsSchedule.Models;
using TimeTableBackend.LessonsSchedule.Services;

namespace TimeTableBackend.LessonsSchedule.LessonsList.Controllers;

[ApiController]
[Route("api/lesson-list")]
public sealed class LessonListController : ControllerBase
{
    private readonly LessonListService _lessonListService;

    public LessonListController(LessonListService lessonListService)
    {
        _lessonListService = lessonListService;
    }

    [HttpGet]
    public async Task<List<Lesson>> Get()
    {
        return await _lessonListService.GetAllAsync();
    }

    [HttpPost]
    public async Task<IActionResult> Post(Lesson newLesson)
    {
        await _lessonListService.CreateAsync(newLesson);

        return CreatedAtAction(nameof(Get), new { id = newLesson.Id }, newLesson);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Lesson updatedLesson)
    {
        var lesson = await _lessonListService.GetByIdAsync(id);

        if (lesson is null)
        {
            return NotFound();
        }

        updatedLesson.Id = lesson.Id;

        await _lessonListService.UpdateAsync(updatedLesson);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var lesson = await _lessonListService.GetByIdAsync(id);

        if (lesson is null)
        {
            return NotFound();
        }

        await _lessonListService.DeleteAsync(id);

        return NoContent();
    }
}