using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Timetable.Api.LessonsSchedule.Controllers;
using Timetable.Api.LessonsSchedule.Dtos;
using Timetable.Api.LessonsSchedule.Entities;
using Timetable.Api.LessonsSchedule.Services;
using Common_DayOfWeek = Timetable.Api.LessonsSchedule.Common.DayOfWeek;
using DayOfWeek = Timetable.Api.LessonsSchedule.Common.DayOfWeek;

namespace Timetable.Api.Tests.Unit;

public sealed class LessonTableControllerTests
{
    private readonly Mock<ILessonTableService> _lessonTablesServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Fixture _fixture;
    private readonly LessonTableController _controller;

    public LessonTableControllerTests()
    {
        _lessonTablesServiceMock = new Mock<ILessonTableService>();
        _mapperMock = new Mock<IMapper>();
        _fixture = new Fixture();
        _controller = new LessonTableController(_lessonTablesServiceMock.Object, _mapperMock.Object);
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
            .Setup(x => x.GetLessonTableByDayOfWeekAsync(It.IsAny<DayOfWeek>()))
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

        _lessonTablesServiceMock.Verify(x => x.UpdateLessonTableAsync(It.Is<LessonTable>(y => y.Equals(lessonTableEntity))));

        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task Post_MakeBackup_ShouldCallService()
    {
        var dayOfWeek = _fixture.Create<DayOfWeek>();

        var response = await _controller.Post_MakeBackup(dayOfWeek);

        _lessonTablesServiceMock.Verify(x => x.MakeLessonTableBackupAsync(It.Is<DayOfWeek>(y => y == dayOfWeek)));

        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task Post_RestoreBackup_WhenBackupDoesntExist_ShouldReturnNotFound()
    {
        var dayOfWeek = _fixture.Create<DayOfWeek>();

        _lessonTablesServiceMock
            .Setup(x => x.RestoreLessonTableFromBackupAsync(It.IsAny<DayOfWeek>()))
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
            .Setup(x => x.RestoreLessonTableFromBackupAsync(It.IsAny<DayOfWeek>()))
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