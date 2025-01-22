using AutoMapper;
using TimeTable.Api.LessonsSchedule.Dtos;
using TimeTable.Api.LessonsSchedule.Entities;

namespace TimeTable.Api.LessonsSchedule.Profiles;

public sealed class LessonProfile : Profile
{
    public LessonProfile()
    {
        CreateMap<Lesson, LessonDto>();
        CreateMap<CreateLessonDto, Lesson>();
        CreateMap<UpdateLessonDto, Lesson>();
    }
}