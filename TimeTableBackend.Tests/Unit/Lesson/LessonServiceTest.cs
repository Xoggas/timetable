using AutoFixture;
using Moq;
using TimeTableBackend.LessonsSchedule.Entities;
using TimeTableBackend.LessonsSchedule.Repositories;
using TimeTableBackend.LessonsSchedule.Services;

namespace TimeTableBackend.Tests.Unit;

public sealed class LessonServiceTest
{
    private readonly Mock<ILessonsRepository> _lessonsRepositoryMock;
    private readonly Mock<IEventService> _eventServiceMock;
    private readonly Fixture _fixture;
    private readonly LessonService _lessonService;

    public LessonServiceTest()
    {
        _lessonsRepositoryMock = new Mock<ILessonsRepository>();
        _eventServiceMock = new Mock<IEventService>();
        _fixture = new Fixture();
        _lessonService = new LessonService(_lessonsRepositoryMock.Object, _eventServiceMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnLessons()
    {
        var lessons = _fixture.CreateMany<Lesson>(5).ToList();

        _lessonsRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(lessons);

        var result = await _lessonService.GetAllAsync();

        Assert.Equal(lessons, result);
    }

    [Fact]
    public async Task GetByIdAsync_WhenLessonExists_ShouldNotReturnNull()
    {
        var id = _fixture.Create<string>();
        var lesson = _fixture
            .Build<Lesson>()
            .With(x => x.Id, id)
            .Create();

        _lessonsRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(lesson);

        var result = await _lessonService.GetByIdAsync(id);

        Assert.Equal(lesson, result);
    }

    [Fact]
    public async Task GetByIdAsync_WhenLessonDoesntExist_ShouldReturnNull()
    {
        var id = _fixture.Create<string>();

        _lessonsRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(default(Lesson));

        var result = await _lessonService.GetByIdAsync(id);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldCallRepositoryAndEventService()
    {
        var lesson = _fixture.Create<Lesson>();

        await _lessonService.CreateAsync(lesson);

        _lessonsRepositoryMock.Verify(x => x.CreateAsync(lesson));

        _eventServiceMock.Verify(x => x.NotifyAllClientsAboutUpdate());
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldCallRepositoryAndEventService()
    {
        var lesson = _fixture.Create<Lesson>();

        await _lessonService.DeleteAsync(lesson);

        _lessonsRepositoryMock.Verify(x => x.DeleteAsync(lesson));

        _eventServiceMock.Verify(x => x.NotifyAllClientsAboutUpdate());
    }
}