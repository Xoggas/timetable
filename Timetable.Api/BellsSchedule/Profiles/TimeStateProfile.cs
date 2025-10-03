using AutoMapper;
using Timetable.Api.BellsSchedule.Dtos;
using Timetable.Api.BellsSchedule.Entities;

namespace Timetable.Api.BellsSchedule.Profiles;

public sealed class TimeStateProfile : Profile
{
    public TimeStateProfile()
    {
        CreateMap<TimeState, TimeStateDto>();
    }
}