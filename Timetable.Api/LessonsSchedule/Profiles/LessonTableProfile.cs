using AutoMapper;
using Timetable.Api.LessonsSchedule.Dtos;
using Timetable.Api.LessonsSchedule.Entities;

namespace Timetable.Api.LessonsSchedule.Profiles;

public class LessonTableProfile : Profile
{
    public LessonTableProfile()
    {
        CreateMap<LessonTable, LessonTableDto>();
        CreateMap<UpdateLessonTableDto, LessonTable>();
    }
}