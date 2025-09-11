using Apex.Domain.Entities;

namespace Apex.Domain.Results;

public class SessionCreationResult
{
    public bool IsSuccess { get; private set; }
    public Session? Session { get; private set; }
    public string? ErrorMessage { get; private set; }
    public bool AlreadyExisted { get; private set; }

    private SessionCreationResult() { }

    public static SessionCreationResult AlreadyExists(Session session)
        => new() { IsSuccess = false, Session = session, AlreadyExisted = true };

    public static SessionCreationResult Created(Session session)
        => new() { IsSuccess = true, Session = session, AlreadyExisted = false };

    public static SessionCreationResult Failed(string errorMessage)
        => new() { IsSuccess = false, ErrorMessage = errorMessage };
}
