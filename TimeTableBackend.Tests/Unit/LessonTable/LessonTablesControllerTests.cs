﻿using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TimeTableBackend.LessonsSchedule.Controllers;
using TimeTableBackend.LessonsSchedule.Dtos;
using TimeTableBackend.LessonsSchedule.Entities;
using TimeTableBackend.LessonsSchedule.Services;
using DayOfWeek = TimeTableBackend.LessonsSchedule.Common.DayOfWeek;

namespace TimeTableBackend.Tests.Unit;

public sealed class LessonTablesControllerTests
{
    private readonly Mock<ILessonTablesService> _lessonTablesServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Fixture _fixture;
    private readonly LessonTablesController _controller;

    public LessonTablesControllerTests()
    {
        _lessonTablesServiceMock = new Mock<ILessonTablesService>();
        _mapperMock = new Mock<IMapper>();
        _fixture = new Fixture();
        _controller = new LessonTablesController(_lessonTablesServiceMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnTable()
    {
        var dayOfWeek = _fixture.Create<DayOfWeek>();
        var lessonTableEntity = _fixture.Create<LessonTable>();
        var lessonTableDto = _fixture
            .Build<LessonTableDto>()
            .With(x => x.DayOfWeek, lessonTableEntity.DayOfWeek)
            .With(x => x.Lessons, lessonTableEntity.Lessons)
            .Create();

        _lessonTablesServiceMock
            .Setup(x => x.GetLessonTableByDayOfWeek(It.IsAny<DayOfWeek>()))
            .ReturnsAsync(lessonTableEntity);

        _mapperMock
            .Setup(x => x.Map<LessonTableDto>(It.IsAny<LessonTable>()))
            .Returns(lessonTableDto);

        var response = await _controller.Get(dayOfWeek);

        var okResult = Assert.IsType<OkObjectResult>(response.Result);

        var returnValue = Assert.IsAssignableFrom<LessonTableDto>(okResult.Value);

        Assert.Equal(returnValue, lessonTableDto);
    }

    [Fact]
    public async Task Put_ShouldCallService()
    {
        var dayOfWeek = _fixture.Create<DayOfWeek>();
        var updateLessonTableDto = _fixture.Create<UpdateLessonTableDto>();
        var lessonTableEntity = _fixture.Create<LessonTable>();

        _mapperMock
            .Setup(x => x.Map<LessonTable>(It.IsAny<UpdateLessonTableDto>()))
            .Callback(() =>
            {
                lessonTableEntity.DayOfWeek = dayOfWeek;
                lessonTableEntity.Lessons = updateLessonTableDto.Lessons;
            })
            .Returns(lessonTableEntity);

        Assert.NotEqual(dayOfWeek, lessonTableEntity.DayOfWeek);

        Assert.NotEqual(updateLessonTableDto.Lessons, lessonTableEntity.Lessons);

        var response = await _controller.Put(dayOfWeek, updateLessonTableDto);

        Assert.Equal(dayOfWeek, lessonTableEntity.DayOfWeek);

        Assert.Equal(updateLessonTableDto.Lessons, lessonTableEntity.Lessons);

        _lessonTablesServiceMock.Verify(x => x.UpdateLessonTable(It.Is<LessonTable>(y => y.Equals(lessonTableEntity))));

        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task Post_MakeBackup_ShouldCallService()
    {
        var dayOfWeek = _fixture.Create<DayOfWeek>();

        var response = await _controller.Post_MakeBackup(dayOfWeek);

        _lessonTablesServiceMock.Verify(x => x.MakeLessonTableBackup(It.Is<DayOfWeek>(y => y == dayOfWeek)));

        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task Post_RestoreBackup_WhenBackupDoesntExist_ShouldReturnNotFound()
    {
        var dayOfWeek = _fixture.Create<DayOfWeek>();

        _lessonTablesServiceMock
            .Setup(x => x.RestoreLessonTableFromBackup(It.IsAny<DayOfWeek>()))
            .ReturnsAsync(default(LessonTable));

        var response = await _controller.Post_RestoreBackup(dayOfWeek);

        Assert.IsType<NotFoundObjectResult>(response.Result);
    }

    [Fact]
    public async Task Post_RestoreBackup_WhenBackupExists_ShouldReturnBackup()
    {
        var dayOfWeek = _fixture.Create<DayOfWeek>();
        var lessonTableEntity = _fixture.Create<LessonTable>();
        var lessonTableDto = _fixture.Build<LessonTableDto>()
            .With(x => x.DayOfWeek, dayOfWeek)
            .With(x => x.Lessons, lessonTableEntity.Lessons)
            .Create();

        _lessonTablesServiceMock
            .Setup(x => x.RestoreLessonTableFromBackup(It.IsAny<DayOfWeek>()))
            .ReturnsAsync(lessonTableEntity);

        _mapperMock
            .Setup(x => x.Map<LessonTableDto>(lessonTableEntity))
            .Returns(lessonTableDto);

        var response = await _controller.Post_RestoreBackup(dayOfWeek);

        var okResult = Assert.IsType<OkObjectResult>(response.Result);

        var returnValue = Assert.IsAssignableFrom<LessonTableDto>(okResult.Value);

        Assert.Equal(returnValue, lessonTableDto);
    }
}