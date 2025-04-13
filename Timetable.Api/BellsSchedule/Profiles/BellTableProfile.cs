using AutoMapper;
using Timetable.Api.BellsSchedule.Dtos;
using Timetable.Api.BellsSchedule.Entities;

namespace Timetable.Api.BellsSchedule.Profiles;

public sealed class BellTableProfile : Profile
{
    public BellTableProfile()
    {
        CreateMap<BellTable, BellTableDto>();
        CreateMap<BellTableDto, BellTable>();
        CreateMap<BellTableRow, BellTableRowDto>();
        CreateMap<BellTableRowDto, BellTableRow>();
        CreateMap<UpdateBellTableDto, BellTable>();
    }
}