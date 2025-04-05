using AutoMapper;
using Timetable.Api.BellsSchedule.Dtos;
using Timetable.Api.BellsSchedule.Entities;

namespace Timetable.Api.BellsSchedule.Profiles;

public sealed class TimeProfile : Profile
{
    public TimeProfile()
    {
        CreateMap<TimeEntity, TimeDto>();
        CreateMap<TimeDto, TimeEntity>();
    }
}