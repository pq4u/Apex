using Apex.Application.Abstractions;
using Apex.Application.DTO;
using Apex.Domain.Entities;
using Apex.Domain.Repositories;

namespace Apex.Application.Queries.Meetings;

public class GetMeetingsQueryHandler : IQueryHandler<GetMeetingsQuery, IEnumerable<Meeting>?>
{
    private readonly IMeetingRepository _meetingRepository;

    public GetMeetingsQueryHandler(IMeetingRepository meetingRepository)
        => _meetingRepository = meetingRepository;

    public async Task<IEnumerable<Meeting>?> HandleAsync(GetMeetingsQuery query)
        => await _meetingRepository.GetAllAsync();
}