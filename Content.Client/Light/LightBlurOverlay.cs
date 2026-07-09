// SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 DoutorWhite <thedoctorwhite@gmail.com>
// SPDX-FileCopyrightText: 2025 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.CCVar;
using Robust.Client.Graphics;
using Robust.Shared.Configuration;
using Robust.Shared.Enums;
using RobustCVars = Robust.Shared.CVars;

namespace Content.Client.Light;

/// <summary>
/// Essentially handles blurring for content-side light overlays.
/// </summary>
public sealed class LightBlurOverlay : Overlay
{
    public override OverlaySpace Space => OverlaySpace.BeforeLighting;

    [Dependency] private readonly IClyde _clyde = default!;
    [Dependency] private readonly IOverlayManager _overlay = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;

    public const int ContentZIndex = TileEmissionOverlay.ContentZIndex + 1;

    private IRenderTarget? _blurTarget;

    public LightBlurOverlay()
    {
        IoCManager.InjectDependencies(this);
        ZIndex = ContentZIndex;
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        if (args.Viewport.Eye is not { } eye || !_cfg.GetCVar(RobustCVars.LightBlur))
            return;

        var beforeOverlay = _overlay.GetOverlay<BeforeLightTargetOverlay>();
        var size = beforeOverlay.EnlargedLightTarget.Size;

        if (_blurTarget?.Size != size)
        {
            _blurTarget = _clyde
                .CreateRenderTarget(size, new RenderTargetFormatParameters(RenderTargetColorFormat.Rgba8Srgb), name: "enlarged-light-blur");
        }

        var target = beforeOverlay.EnlargedLightTarget;
        _clyde.BlurRenderTarget(args.Viewport, target, _blurTarget, eye, 14f);
    }
}