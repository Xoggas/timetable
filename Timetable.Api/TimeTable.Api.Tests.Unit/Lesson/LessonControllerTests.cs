using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Timetable.Api.LessonsSchedule.Controllers;
using Timetable.Api.LessonsSchedule.Dtos;
using Timetable.Api.LessonsSchedule.Entities;
using Timetable.Api.LessonsSchedule.Services;

namespace Timetable.Api.Tests.Unit;

public sealed class LessonControllerTests
{
    private readonly Mock<ILessonsService> _lessonsServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly LessonController _controller;
    private readonly Fixture _fixture;

    public LessonControllerTests()
    {
        _lessonsServiceMock = new Mock<ILessonsService>();
        _mapperMock = new Mock<IMapper>();
        _controller = new LessonController(_lessonsServiceMock.Object, _mapperMock.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Get_ShouldReturnOneLesson()
    {
        var lessonEntity = _fixture.Create<Lesson>();

        var lessonDto = _fixture
            .Build<LessonDto>()
            .With(x => x.Id, lessonEntity.Id)
            .With(x => x.Name, lessonEntity.Name)
            .Create();

        _mapperMock
            .Setup(x => x.Map<IEnumerable<LessonDto>>(It.IsAny<IEnumerable<Lesson>>()))
            .Returns([lessonDto]);

        _lessonsServiceMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync([lessonEntity]);

        var response = await _controller.Get();

        var result = Assert.IsType<OkObjectResult>(response.Result);

        var lessons = Assert.IsAssignableFrom<IEnumerable<LessonDto>>(result.Value);

        Assert.Equal(lessons, [lessonDto]);
    }

    [Fact]
    public async Task Post_ShouldReturnOneLesson()
    {
        var lessonEntity = _fixture.Create<Lesson>();

        var createLessonDto = _fixture
            .Build<CreateLessonDto>()
            .With(x => x.Name, lessonEntity.Name)
            .Create();

        var lessonDto = _fixture
            .Build<LessonDto>()
            .With(x => x.Id, lessonEntity.Id)
            .With(x => x.Name, lessonEntity.Name)
            .Create();

        _mapperMock
            .Setup(x => x.Map<LessonDto>(It.IsAny<Lesson>()))
            .Returns(lessonDto);

        _mapperMock
            .Setup(x => x.Map<Lesson>(It.IsAny<CreateLessonDto>()))
            .Returns(lessonEntity);

        var response = await _controller.Post(createLessonDto);

        var result = Assert.IsType<OkObjectResult>(response.Result);

        var createdLesson = Assert.IsAssignableFrom<LessonDto>(result.Value);

        Assert.Equal(createdLesson, lessonDto);
    }

    [Fact]
    public async Task Put_WhenIdIsIncorrect_ShouldReturnNotFound()
    {
        var id = _fixture.Create<string>();
        var updateLessonDto = _fixture.Create<UpdateLessonDto>();

        _lessonsServiceMock
            .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(default(Lesson));

        var response = await _controller.Put(id, updateLessonDto);

        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task Put_WhenIdIsCorrect_ShouldReturnUpdatedLesson()
    {
        var id = _fixture.Create<string>();
        var updateLessonDto = _fixture.Create<UpdateLessonDto>();

        var lessonEntity = _fixture
            .Build<Lesson>()
            .With(x => x.Id, id)
            .Create();

        _lessonsServiceMock
            .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(lessonEntity);

        _mapperMock
            .Setup(x => x.Map(It.IsAny<UpdateLessonDto>(), It.IsAny<Lesson>()))
            .Callback(() => lessonEntity.Name = updateLessonDto.Name);

        Assert.NotEqual(lessonEntity.Name, updateLessonDto.Name);

        var response = await _controller.Put(id, updateLessonDto);

        Assert.Equal(lessonEntity.Name, updateLessonDto.Name);

        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task Delete_WhenIdIsIncorrect_ShouldReturnNotFound()
    {
        var id = _fixture.Create<string>();

        _lessonsServiceMock
            .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(default(Lesson));

        var result = await _controller.Delete(id);

        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task Delete_WhenIdIsCorrect_ShouldReturnNoContent()
    {
        var id = _fixture.Create<string>();
        var lessonEntity = _fixture.Create<Lesson>();

        _lessonsServiceMock
            .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(lessonEntity);

        var result = await _controller.Delete(id);

        Assert.IsType<NoContentResult>(result);
    }
}