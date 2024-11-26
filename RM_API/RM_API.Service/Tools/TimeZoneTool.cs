namespace RM_API.Service.Tools;

public class TimeZoneTool
{
    private readonly TimeZoneInfo _appTimeZone;

    public TimeZoneTool(string appTimeZone)
    {
        _appTimeZone = TimeZoneInfo.FindSystemTimeZoneById(appTimeZone);
    }
        
    public DateTime ConvertUtcToAppTimeZone(DateTime utcDate)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(utcDate, _appTimeZone);
    }

    public DateTime ConvertAppTimeZoneToUtc(DateTime appTimeZoneDate)
    {
        return TimeZoneInfo.ConvertTimeToUtc(appTimeZoneDate, _appTimeZone);
    }
}