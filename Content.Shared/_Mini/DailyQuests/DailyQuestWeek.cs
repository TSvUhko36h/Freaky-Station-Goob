using System;

namespace Content.Shared._Mini.DailyQuests;

/// <summary>
/// Helpers for the weekly quest period key (Monday 00:00 MSK calendar date).
/// </summary>
public static class DailyQuestWeek
{
    private static readonly TimeZoneInfo MoscowTimeZone = InitializeMoscowTimeZone();

    /// <summary>
    /// Start of the current quest week (Monday, MSK).
    /// </summary>
    public static DateTime GetCurrentWeekStart()
    {
        var moscowDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, MoscowTimeZone).Date;
        var daysFromMonday = ((int)moscowDate.DayOfWeek + 6) % 7;
        return moscowDate.AddDays(-daysFromMonday);
    }

    /// <summary>
    /// Canonical storage key for <c>daily_quest_progress.quest_date</c>.
    /// Uses UTC midnight of the MSK calendar Monday so timestamptz round-trips reliably.
    /// </summary>
    public static DateTime ToStoredKey(DateTime weekStart)
        => DateTime.SpecifyKind(weekStart.Date, DateTimeKind.Utc);

    public static bool Matches(DateTime stored, DateTime currentWeekStart)
        => ToStoredKey(NormalizeToWeekStart(stored)) == ToStoredKey(currentWeekStart);

    public static DateTime GetNextResetUtc(DateTime weekStart)
    {
        var nextWeekStart = DateTime.SpecifyKind(weekStart.Date.AddDays(7), DateTimeKind.Unspecified);
        return TimeZoneInfo.ConvertTimeToUtc(nextWeekStart, MoscowTimeZone);
    }

    private static DateTime NormalizeToWeekStart(DateTime value)
    {
        var moscowDate = value.Kind == DateTimeKind.Utc
            ? TimeZoneInfo.ConvertTimeFromUtc(value, MoscowTimeZone).Date
            : value.Date;

        var daysFromMonday = ((int)moscowDate.DayOfWeek + 6) % 7;
        return moscowDate.AddDays(-daysFromMonday);
    }

    private static TimeZoneInfo InitializeMoscowTimeZone()
    {
        foreach (var id in new[] { "Europe/Moscow", "Russian Standard Time" })
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(id);
            }
            catch (TimeZoneNotFoundException)
            {
            }
        }

        return TimeZoneInfo.CreateCustomTimeZone(
            "MSK",
            TimeSpan.FromHours(3),
            "Moscow Standard Time",
            "Moscow Standard Time");
    }
}
