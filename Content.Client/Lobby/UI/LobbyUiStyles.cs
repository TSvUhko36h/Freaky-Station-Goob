// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Client.Graphics;
using Robust.Shared.Maths;

namespace Content.Client.Lobby.UI;

/// <summary>
/// Shared visual constants for lobby character setup UI, aligned with the liquid-glass theme.
/// </summary>
public static class LobbyUiStyles
{
    public const float SelectedButtonAlpha = 0.88f;

    public static readonly Color GlassPanel = Color.FromHex("#2A2A38D9");
    public static readonly Color GlassHeading = Color.FromHex("#323240CC");
    public static readonly Color GlassSetupBackground = Color.FromHex("#14141CCC");
    public static readonly Color GlassAccentLine = Color.FromHex("#A88B5E80");

    public static StyleBoxFlat GlassPanelBox() => new()
    {
        BackgroundColor = GlassPanel,
        BorderThickness = new Thickness(0),
        ContentMarginTopOverride = 10,
        ContentMarginBottomOverride = 10,
        ContentMarginLeftOverride = 10,
        ContentMarginRightOverride = 10,
    };

    public static StyleBoxFlat SectionHeader() => new()
    {
        BackgroundColor = GlassHeading,
        BorderThickness = new Thickness(0),
        ContentMarginLeftOverride = 8,
        ContentMarginTopOverride = 4,
        ContentMarginRightOverride = 8,
        ContentMarginBottomOverride = 4,
    };

    public static StyleBoxFlat SelectedPriorityButton(Color color) => new()
    {
        BackgroundColor = color.WithAlpha(SelectedButtonAlpha),
        BorderThickness = new Thickness(0),
        Padding = new Thickness(2),
        ContentMarginLeftOverride = 10,
        ContentMarginTopOverride = 2,
        ContentMarginRightOverride = 10,
        ContentMarginBottomOverride = 2,
    };
}
