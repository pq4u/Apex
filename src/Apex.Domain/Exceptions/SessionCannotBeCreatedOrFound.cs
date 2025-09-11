namespace Apex.Domain.Exceptions;

public class SessionCannotBeCreatedOrFound : CustomException
{
    public int SessionKey { get; set; }
    public SessionCannotBeCreatedOrFound(int sessionKey) : base($"Session {sessionKey} could not be created or found")
    {
        SessionKey = sessionKey;
    }
}
