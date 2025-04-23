using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Timetable.Api.LessonsSchedule.Common;
using Timetable.Api.LessonsSchedule.Dtos;
using Timetable.Api.LessonsSchedule.Entities;
using Timetable.Api.LessonsSchedule.Services;

namespace Timetable.Api.LessonsSchedule.Controllers;

[ApiController]
[Route("api/lesson-table/{customDayOfWeek:dayofweek}")]
[DisplayName("Lesson Table Controller")]
[Produces("application/json")]
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
    /// <param name="customDayOfWeek">The day of the week for which the lesson table is retrieved.</param>
    /// <returns>The lesson table for the specified day.</returns>
    /// <response code="200">Returns the lesson table for the given day.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<LessonTableDto>> Get(CustomDayOfWeek customDayOfWeek)
    {
        var lessonTableEntity = await _lessonTableService.GetLessonTableByDayOfWeekAsync(customDayOfWeek);

        return Ok(_mapper.Map<LessonTableDto>(lessonTableEntity));
    }
    
    [HttpGet("csv")]
    public async Task<FileContentResult> GetCsv(CustomDayOfWeek customDayOfWeek)
    {
        var csv = await _lessonTableService.GetCsvLessonTableByDayOfWeekAsync(customDayOfWeek);
        return File(csv, "text/csv", $"{customDayOfWeek.ToString()}.csv");
    }
    
    [HttpGet("json")]
    public async Task<FileContentResult> GetJson(CustomDayOfWeek customDayOfWeek)
    {
        var json = await _lessonTableService.GetJsonLessonTableByDayOfWeekAsync(customDayOfWeek);
        return File(json, "text/json", $"{customDayOfWeek.ToString()}.json");
    }

    /// <summary>
    /// Updates the lesson table for a specific day of the week.
    /// </summary>
    /// <param name="customDayOfWeek">The day of the week to update the lesson table for.</param>
    /// <param name="dto">The updated lesson table data.</param>
    /// <response code="204">The lesson table was successfully updated.</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Put(CustomDayOfWeek customDayOfWeek, UpdateLessonTableDto dto)
    {
        var lessonTableEntity = _mapper.Map<LessonTable>(dto);

        lessonTableEntity.DayOfWeek = customDayOfWeek;

        await _lessonTableService.UpdateLessonTableAsync(lessonTableEntity);

        return NoContent();
    }

    /// <summary>
    /// Creates a backup of the lesson table for a specific day of the week.
    /// </summary>
    /// <param name="customDayOfWeek">The day of the week to create a backup for.</param>
    /// <response code="204">Backup was successfully created.</response>
    [HttpPost("backup")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Post_MakeBackup(CustomDayOfWeek customDayOfWeek)
    {
        await _lessonTableService.MakeLessonTableBackupAsync(customDayOfWeek);

        return NoContent();
    }

    /// <summary>
    /// Restores the lesson table for a specific day of the week from a backup.
    /// </summary>
    /// <param name="customDayOfWeek">The day of the week to restore the lesson table for.</param>
    /// <returns>The restored lesson table.</returns>
    /// <response code="200">Returns the restored lesson table.</response>
    /// <response code="404">No backup found for the specified day.</response>
    [HttpPost("restore")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LessonTableDto>> Post_RestoreBackup(CustomDayOfWeek customDayOfWeek)
    {
        var backup = await _lessonTableService.RestoreLessonTableFromBackupAsync(customDayOfWeek);

        if (backup is null)
        {
            return NotFound($"No backup for {customDayOfWeek:G}");
        }

        return Ok(_mapper.Map<LessonTableDto>(backup));
    }
}