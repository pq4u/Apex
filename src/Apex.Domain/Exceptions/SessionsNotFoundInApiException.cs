namespace Apex.Domain.Exceptions;

public class SessionsInMeetingNotFoundInApiException : CustomException
{
    public int MeetingKey { get; set; }
    public SessionsInMeetingNotFoundInApiException(int meetingKey) : base($"Sessions from meeting key {meetingKey} not found in API")
    {
        MeetingKey = meetingKey;
    }
}
