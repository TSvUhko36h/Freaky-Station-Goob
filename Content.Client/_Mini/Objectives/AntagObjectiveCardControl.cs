using System;
using System.Numerics;
using Content.Client._Mini.UserInterface;
using Content.Client.Resources;
using Content.Client.UserInterface;
using Content.Shared._Mini.AntagTokens;
using Content.Shared.Objectives;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.Utility;
using Robust.Shared.IoC;
using Robust.Shared.Localization;
using Robust.Shared.Maths;
using Robust.Shared.Utility;

namespace Content.Client._Mini.Objectives;

/// <summary>
/// Antagonist objective card styled like compact daily quest cards.
/// </summary>
public sealed class AntagObjectiveCardControl : BoxContainer
{
    private readonly PixelTiledProgressBar _progressBar;
    private readonly Label _progressLabel;
    private readonly Label? _statusLabel;
    private readonly bool _showStatus;

    private string? _boundTitle;
    private string? _boundDescription;
    private float _boundProgress = -1f;
    private bool _boundComplete;
    private Label? _rewardFooterLabel;
    private PanelContainer? _rewardFooterPanel;

    public AntagObjectiveCardControl(bool showStatus = false)
    {
        _showStatus = showStatus;

        Orientation = LayoutOrientation.Vertical;
        SeparationOverride = 0;
        HorizontalExpand = true;
        VerticalExpand = false;

        _progressBar = new PixelTiledProgressBar();
        _progressLabel = new Label
        {
            Modulate = CharacterMenuCardStyle.ProgressTextColor,
            HorizontalAlignment = HAlignment.Right,
        };

        if (showStatus)
        {
            _statusLabel = new Label
            {
                HorizontalAlignment = HAlignment.Center,
                Modulate = Color.FromHex("#e8eef8"),
            };
        }
    }

    public void SetObjective(ObjectiveInfo info, Texture? icon)
    {
        var complete = info.Progress >= 0.99f;

        if (!NeedsObjectiveRebuild(info))
        {
            UpdateObjectiveProgress(info);
            return;
        }

        _boundTitle = info.Title;
        _boundDescription = info.Description;
        _boundProgress = info.Progress;
        _boundComplete = complete;

        DetachReusedControls();
        RemoveAllChildren();

        var panel = CreateCardPanel(complete);

        var column = new BoxContainer
        {
            Orientation = LayoutOrientation.Vertical,
            SeparationOverride = 4,
            HorizontalExpand = true,
            VerticalExpand = false,
        };
        panel.AddChild(column);
        AddChild(panel);

        var header = new BoxContainer
        {
            Orientation = LayoutOrientation.Horizontal,
            SeparationOverride = 8,
            VerticalAlignment = VAlignment.Center,
            HorizontalExpand = true,
        };
        column.AddChild(header);

        if (icon != null)
        {
            header.AddChild(new TextureRect
            {
                Texture = icon,
                MinSize = new Vector2(CharacterMenuCardStyle.IconSize, CharacterMenuCardStyle.IconSize),
                Stretch = TextureRect.StretchMode.KeepAspectCentered,
                VerticalAlignment = VAlignment.Center,
            });
        }

        header.AddChild(CharacterMenuCardStyle.CreateTitleMarqueeHost(info.Title));

        if (!string.IsNullOrWhiteSpace(info.Description))
            column.AddChild(CharacterMenuCardStyle.CreateBodyMarqueeHost(info.Description));

        _progressBar.Value = Math.Clamp(info.Progress, 0f, 1f);
        _progressLabel.Text = FormatProgress(info.Progress);

        column.AddChild(_progressBar);
        column.AddChild(_progressLabel);

        if (_showStatus && _statusLabel != null)
        {
            _statusLabel.Text = complete
                ? Loc.GetString("antag-objective-complete")
                : Loc.GetString("antag-objective-active");
            _statusLabel.Modulate = complete
                ? CharacterMenuCardStyle.ClaimReadyColor
                : Color.FromHex("#e8eef8");
            column.AddChild(_statusLabel);
        }
    }

    public void UpdateObjectiveProgress(ObjectiveInfo info)
    {
        if (ChildCount == 0)
        {
            SetObjective(info, null);
            return;
        }

        _boundProgress = info.Progress;
        var complete = info.Progress >= 0.99f;

        _progressBar.Value = Math.Clamp(info.Progress, 0f, 1f);
        _progressLabel.Text = FormatProgress(info.Progress);

        if (_showStatus && _statusLabel != null)
        {
            _statusLabel.Text = complete
                ? Loc.GetString("antag-objective-complete")
                : Loc.GetString("antag-objective-active");
            _statusLabel.Modulate = complete
                ? CharacterMenuCardStyle.ClaimReadyColor
                : Color.FromHex("#e8eef8");
        }

        if (_boundComplete != complete
            && GetChild(0) is PanelContainer panel
            && panel.PanelOverride is StyleBoxFlat style)
        {
            _boundComplete = complete;
            style.BorderColor = complete
                ? CharacterMenuCardStyle.ClaimReadyColor.WithAlpha(0.4f)
                : CharacterMenuCardStyle.ObjectiveBorderColor.WithAlpha(0.35f);
        }
    }

    public void SetBriefing(string briefing)
    {
        DetachReusedControls();
        RemoveAllChildren();

        var panel = CreateBriefingPanel();
        panel.AddChild(CharacterMenuCardStyle.CreateBriefingMarqueeHost(briefing));
        AddChild(panel);
    }

    public void SetRewardFooter(bool allComplete, bool rewardGranted)
    {
        if (_rewardFooterPanel != null && _rewardFooterLabel != null)
        {
            UpdateRewardFooter(allComplete, rewardGranted);
            return;
        }

        DetachReusedControls();
        RemoveAllChildren();

        var cache = IoCManager.Resolve<IResourceCache>();
        var coinTexture = cache.TryGetResource<TextureResource>(
                new ResPath(AntagTokenCatalog.CurrencyIconPath), out var antagRes)
            ? antagRes.Texture
            : cache.GetTexture("/Textures/_Mini/Interface/Coin.png");

        var complete = allComplete || rewardGranted;
        _rewardFooterPanel = CreateRewardPanel(complete);

        var row = new BoxContainer
        {
            Orientation = LayoutOrientation.Horizontal,
            SeparationOverride = 6,
            HorizontalAlignment = HAlignment.Center,
            VerticalAlignment = VAlignment.Center,
        };

        _rewardFooterLabel = new Label
        {
            Modulate = rewardGranted ? CharacterMenuCardStyle.ClaimReadyColor : CharacterMenuCardStyle.ProgressTextColor,
            VerticalAlignment = VAlignment.Center,
        };

        row.AddChild(_rewardFooterLabel);
        row.AddChild(new TextureRect
        {
            Texture = coinTexture,
            MinSize = new Vector2(18, 18),
            TextureScale = new Vector2(0.38f, 0.38f),
            Stretch = TextureRect.StretchMode.KeepAspectCentered,
            VerticalAlignment = VAlignment.Center,
        });

        _rewardFooterPanel.AddChild(row);
        AddChild(_rewardFooterPanel);
        UpdateRewardFooter(allComplete, rewardGranted);
    }

    public void UpdateRewardFooter(bool allComplete, bool rewardGranted)
    {
        if (_rewardFooterPanel == null || _rewardFooterLabel == null)
        {
            SetRewardFooter(allComplete, rewardGranted);
            return;
        }

        var complete = allComplete || rewardGranted;
        _rewardFooterLabel.Text = rewardGranted
            ? Loc.GetString("antag-objective-reward-granted")
            : Loc.GetString("antag-objective-reward-pending");
        _rewardFooterLabel.Modulate = rewardGranted
            ? CharacterMenuCardStyle.ClaimReadyColor
            : CharacterMenuCardStyle.ProgressTextColor;

        if (_rewardFooterPanel.PanelOverride is StyleBoxFlat)
            _rewardFooterPanel.PanelOverride = CreateRewardPanelStyle(complete);
    }

    private bool NeedsObjectiveRebuild(ObjectiveInfo info)
    {
        return ChildCount == 0
            || _boundTitle != info.Title
            || _boundDescription != info.Description;
    }

    private void DetachReusedControls()
    {
        _progressBar.Orphan();
        _progressLabel.Orphan();
        _statusLabel?.Orphan();
    }

    private static PanelContainer CreateCardPanel(bool complete, float minHeight = 0) => new()
    {
        HorizontalExpand = true,
        VerticalExpand = false,
        RectClipContent = true,
        MinSize = minHeight > 0 ? new Vector2(0, minHeight) : default,
        PanelOverride = CharacterMenuCardStyle.CreateObjectivePanelStyle(complete),
    };

    private static PanelContainer CreateRewardPanel(bool complete) => new()
    {
        HorizontalExpand = true,
        VerticalExpand = false,
        RectClipContent = true,
        MinSize = new Vector2(0, 44),
        PanelOverride = CreateRewardPanelStyle(complete),
    };

    private static StyleBoxFlat CreateRewardPanelStyle(bool complete) => new()
    {
        BackgroundColor = CharacterMenuCardStyle.CardBackgroundColor.WithAlpha(0.55f),
        BorderColor = complete
            ? CharacterMenuCardStyle.ClaimReadyColor.WithAlpha(0.45f)
            : CharacterMenuCardStyle.AccentColor.WithAlpha(0.3f),
        BorderThickness = new Thickness(1),
        ContentMarginLeftOverride = 8,
        ContentMarginTopOverride = 6,
        ContentMarginRightOverride = 8,
        ContentMarginBottomOverride = 6,
    };

    private static PanelContainer CreateBriefingPanel() => new()
    {
        HorizontalExpand = true,
        VerticalExpand = false,
        RectClipContent = true,
        PanelOverride = new StyleBoxFlat
        {
            BackgroundColor = CharacterMenuCardStyle.CardBackgroundColor.WithAlpha(0.55f),
            BorderColor = CharacterMenuCardStyle.AccentColor.WithAlpha(0.35f),
            BorderThickness = new Thickness(1),
            ContentMarginLeftOverride = 8,
            ContentMarginTopOverride = 6,
            ContentMarginRightOverride = 8,
            ContentMarginBottomOverride = 6,
        },
    };

    private static string FormatProgress(float progress)
    {
        var percent = (int)Math.Round(Math.Clamp(progress, 0f, 1f) * 100f);
        return Loc.GetString("antag-objective-progress", ("percent", percent));
    }
}
