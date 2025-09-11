namespace Apex.Domain.Exceptions;

public class SessionNotFoundInApiException : CustomException
{
    public int SessionKey { get; set; }
    public SessionNotFoundInApiException(int sessionKey) : base($"Session {sessionKey} not found in API")
    {
        SessionKey = sessionKey;
    }
}
