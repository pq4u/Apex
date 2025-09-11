namespace Apex.Domain.Exceptions;

public class MeetingNotFoundInApiException : CustomException
{
    public int MeetingKey { get; set; }
    public MeetingNotFoundInApiException(int meetingKey) : base($"Meeting {meetingKey} not found in API")
    {
        MeetingKey = meetingKey;
    }
}
