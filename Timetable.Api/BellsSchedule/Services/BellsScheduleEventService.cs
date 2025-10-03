using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Timetable.Api.BellsSchedule.Dtos;
using Timetable.Api.BellsSchedule.Entities;
using Timetable.Api.BellsSchedule.Hubs;

namespace Timetable.Api.Shared.Services;

public interface IBellsScheduleEventService
{
    Task NotifyAllClientsAboutUpdate(BellTable bellTable);
    Task NotifyAllClientsAboutTimeStateChange(TimeStateDto dto);
}

public sealed class BellsScheduleEventService : IBellsScheduleEventService
{
    private readonly IHubContext<BellsScheduleEventHub, IBellsScheduleEventHub> _eventHub;
    private readonly IMapper _mapper;

    public BellsScheduleEventService(IHubContext<BellsScheduleEventHub, IBellsScheduleEventHub> eventHub,
        IMapper mapper)
    {
        _eventHub = eventHub;
        _mapper = mapper;
    }

    public async Task NotifyAllClientsAboutUpdate(BellTable bellTable)
    {
        var dto = _mapper.Map<BellTableDto>(bellTable);
        await _eventHub.Clients.All.Update(dto);
    }

    public async Task NotifyAllClientsAboutTimeStateChange(TimeStateDto dto)
    {
        await _eventHub.Clients.All.TimeStateChange(dto);
    }
}