using System.ComponentModel;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimeTableBackend.LessonsSchedule.Dtos;
using TimeTableBackend.LessonsSchedule.Entities;
using TimeTableBackend.LessonsSchedule.Services;
using DayOfWeek = TimeTableBackend.LessonsSchedule.Common.DayOfWeek;

namespace TimeTableBackend.LessonsSchedule.Controllers;

[ApiController]
[Route("api/lesson-table/{dayOfWeek}")]
[DisplayName("Lesson Table Controller")]
public class LessonTableController : ControllerBase
{
    private readonly ILessonTableService _lessonTableService;
    private readonly IMapper _mapper;

    public LessonTableController(ILessonTableService lessonTableService, IMapper mapper)
    {
        _lessonTableService = lessonTableService;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves the lesson table for a specific day of the week.
    /// </summary>
    /// <param name="dayOfWeek">The day of the week for which the lesson table is retrieved.</param>
    /// <returns>The lesson table for the specified day.</returns>
    /// <response code="200">Returns the lesson table for the given day.</response>
    [HttpGet]
    public async Task<ActionResult<LessonTableDto>> Get(DayOfWeek dayOfWeek)
    {
        var lessonTableEntity = await _lessonTableService.GetLessonTableByDayOfWeekAsync(dayOfWeek);

        return Ok(_mapper.Map<LessonTableDto>(lessonTableEntity));
    }

    /// <summary>
    /// Updates the lesson table for a specific day of the week.
    /// </summary>
    /// <param name="dayOfWeek">The day of the week to update the lesson table for.</param>
    /// <param name="dto">The updated lesson table data.</param>
    /// <response code="204">The lesson table was successfully updated.</response>
    [HttpPut]
    public async Task<ActionResult> Put(DayOfWeek dayOfWeek, UpdateLessonTableDto dto)
    {
        var lessonTableEntity = _mapper.Map<LessonTable>(dto);

        lessonTableEntity.DayOfWeek = dayOfWeek;

        await _lessonTableService.UpdateLessonTableAsync(lessonTableEntity);

        return NoContent();
    }

    /// <summary>
    /// Creates a backup of the lesson table for a specific day of the week.
    /// </summary>
    /// <param name="dayOfWeek">The day of the week to create a backup for.</param>
    /// <response code="204">Backup was successfully created.</response>
    [HttpPost("backup")]
    public async Task<ActionResult> Post_MakeBackup(DayOfWeek dayOfWeek)
    {
        await _lessonTableService.MakeLessonTableBackupAsync(dayOfWeek);

        return NoContent();
    }

    /// <summary>
    /// Restores the lesson table for a specific day of the week from a backup.
    /// </summary>
    /// <param name="dayOfWeek">The day of the week to restore the lesson table for.</param>
    /// <returns>The restored lesson table.</returns>
    /// <response code="200">Returns the restored lesson table.</response>
    /// <response code="404">No backup found for the specified day.</response>
    [HttpPost("restore")]
    public async Task<ActionResult<LessonTableDto>> Post_RestoreBackup(DayOfWeek dayOfWeek)
    {
        var backup = await _lessonTableService.RestoreLessonTableFromBackupAsync(dayOfWeek);

        if (backup is null)
        {
            return NotFound($"No backup for {dayOfWeek:G}");
        }

        return Ok(_mapper.Map<LessonTableDto>(backup));
    }
}