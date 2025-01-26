using AutoMapper;
using Timetable.Api.LessonsSchedule.Dtos;
using Timetable.Api.LessonsSchedule.Entities;

namespace Timetable.Api.LessonsSchedule.Profiles;

public sealed class LessonProfile : Profile
{
    public LessonProfile()
    {
        CreateMap<Lesson, LessonDto>();
        CreateMap<CreateLessonDto, Lesson>();
        CreateMap<UpdateLessonDto, Lesson>();
    }
}