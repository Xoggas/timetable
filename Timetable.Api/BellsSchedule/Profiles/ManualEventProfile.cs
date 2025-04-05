using AutoMapper;
using Timetable.Api.BellsSchedule.Dtos;
using Timetable.Api.BellsSchedule.Entities;

namespace Timetable.Api.BellsSchedule.Profiles;

public sealed class ManualEventProfile : Profile
{
    public ManualEventProfile()
    {
        CreateMap<ManualEvent, ManualEventDto>();
        CreateMap<UpdateManualEventDto, ManualEvent>();
    }
}