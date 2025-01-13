using AutoMapper;
using TimeTableBackend.LessonsSchedule.Dtos;
using TimeTableBackend.LessonsSchedule.Entities;

namespace TimeTableBackend.LessonsSchedule.Profiles;

public sealed class LessonProfile : Profile
{
    public LessonProfile()
    {
        CreateMap<Lesson, LessonDto>();
        CreateMap<CreateLessonDto, Lesson>();
        CreateMap<UpdateLessonDto, Lesson>();
    }
}