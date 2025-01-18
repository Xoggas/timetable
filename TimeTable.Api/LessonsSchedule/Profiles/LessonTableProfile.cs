using AutoMapper;
using TimeTable.Api.LessonsSchedule.Dtos;
using TimeTable.Api.LessonsSchedule.Entities;

namespace TimeTable.Api.LessonsSchedule.Profiles;

public class LessonTableProfile : Profile
{
    public LessonTableProfile()
    {
        CreateMap<LessonTable, LessonTableDto>();
        CreateMap<UpdateLessonTableDto, LessonTable>();
    }
}