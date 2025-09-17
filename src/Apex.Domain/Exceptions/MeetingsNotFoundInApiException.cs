namespace Apex.Domain.Exceptions;

public class MeetingsNotFoundInApiException : CustomException
{
    public MeetingsNotFoundInApiException() : base($"No meetings found in API")
    {
    }
}
