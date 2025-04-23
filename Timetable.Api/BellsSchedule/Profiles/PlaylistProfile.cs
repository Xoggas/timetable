using AutoMapper;
using Timetable.Api.BellsSchedule.Dtos;
using Timetable.Api.BellsSchedule.Entities;

namespace Timetable.Api.BellsSchedule.Profiles;

public sealed class PlaylistProfile : Profile
{
    public PlaylistProfile()
    {
        CreateMap<Playlist, PlaylistDto>();
        CreateMap<UpdatePlaylistDto, Playlist>();
    }
}