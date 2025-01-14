using AutoMapper;
using TimeTableBackend.LessonsSchedule.Dtos;
using TimeTableBackend.LessonsSchedule.Entities;

namespace TimeTableBackend.LessonsSchedule.Profiles;

public class LessonTableProfile : Profile
{
    public LessonTableProfile()
    {
        CreateMap<LessonTable, LessonTableDto>();
    }
}