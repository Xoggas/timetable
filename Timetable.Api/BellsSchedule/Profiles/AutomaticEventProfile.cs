using AutoMapper;
using Timetable.Api.BellsSchedule.Dtos;
using Timetable.Api.BellsSchedule.Entities;

namespace Timetable.Api.BellsSchedule.Profiles;

public class AutomaticEventProfile : Profile
{
    public AutomaticEventProfile()
    {
        CreateMap<AutomaticEvent, AutomaticEventDto>();
        CreateMap<UpdateAutomaticEventDto, AutomaticEvent>();
    }
}