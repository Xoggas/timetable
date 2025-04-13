using AutoMapper;
using Timetable.Api.BellsSchedule.Dtos;
using Timetable.Api.BellsSchedule.Entities;

namespace Timetable.Api.BellsSchedule.Profiles;

public sealed class SoundFileProfile : Profile
{
    public SoundFileProfile()
    {
        CreateMap<SoundFile, SoundFileDto>();
        CreateMap<UpdateSoundFileDto, SoundFile>();
    }
}