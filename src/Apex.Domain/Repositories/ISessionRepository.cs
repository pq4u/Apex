using Apex.Domain.Entities;

namespace Apex.Domain.Repositories;

public interface ISessionRepository
{
    Task<IEnumerable<Session>?> GetAllAsync();
    Task<IEnumerable<Session>?> GetAllRacesAsync();
    Task<IEnumerable<Session>?> GetAllSessionsInMeetingAsync(int meetingId);
    Task AddAsync(Session session);
    Task<bool> ExistsByKeyAsync(int key);
}
