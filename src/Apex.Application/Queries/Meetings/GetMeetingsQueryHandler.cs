using Apex.Application.Abstractions;
using Apex.Application.DTO;
using Apex.Domain.Entities;
using Apex.Domain.Repositories;

namespace Apex.Application.Queries.Meetings;

public class GetMeetingsQueryHandler : IQueryHandler<GetMeetingsQuery, List<Meeting>?>
{
    private readonly IMeetingRepository _meetingRepository;

    public GetMeetingsQueryHandler(IMeetingRepository meetingRepository)
        => _meetingRepository = meetingRepository;

    public async Task<List<Meeting>?> HandleAsync(GetMeetingsQuery query, CancellationToken cancellationToken = default)
        => await _meetingRepository.GetAllAsync(cancellationToken);
}