using Robust.Shared.Localization;

namespace Content.Shared.Localization;

public static class LocExtensions
{
    public static string LocalizeOrRaw(string? messageId)
    {
        if (string.IsNullOrEmpty(messageId))
            return string.Empty;

        return Loc.TryGetString(messageId, out var localized) ? localized : messageId;
    }
}
