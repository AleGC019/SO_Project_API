namespace RM_API.Service.Tools;

public class TimeZoneTool
{
    private readonly TimeZoneInfo _appTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

    // Set application-wide time zone, for example, Eastern Time

    public DateTime ConvertUtcToAppTimeZone(DateTime utcDate)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(utcDate, _appTimeZone);
    }

    public DateTime ConvertAppTimeZoneToUtc(DateTime appTimeZoneDate)
    {
        return TimeZoneInfo.ConvertTimeToUtc(appTimeZoneDate, _appTimeZone);
    }
}