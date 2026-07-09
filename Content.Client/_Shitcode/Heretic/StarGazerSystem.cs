using Content.Shared._Shitcode.Heretic.Components;
using Content.Shared._Shitcode.Heretic.Systems;
using Content.Shared.Heretic;
using Robust.Client.Graphics;
using Robust.Client.Input;
using Robust.Client.Player;
using Robust.Shared.Map;
using System.Numerics;

namespace Content.Client._Shitcode.Heretic;

public sealed class StarGazerSystem : SharedStarGazerSystem
{
    private const float UpdateInterval = 0.05f;

    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly IEyeManager _eye = default!;
    [Dependency] private readonly IInputManager _input = default!;

    private float _updateAccumulator;
    private MapCoordinates? _lastMousePos;

    public override void FrameUpdate(float frameTime)
    {
        base.FrameUpdate(frameTime);

        if (!Timing.IsFirstTimePredicted)
            return;

        if (!HasComp<StarGazeComponent>(_player.LocalEntity))
            return;

        var player = _player.LocalEntity.Value;

        var mousePos = _eye.PixelToMap(_input.MouseScreenPosition);

        if (mousePos.MapId == MapId.Nullspace)
            return;

        _updateAccumulator += frameTime;

        if (_lastMousePos is { } last
            && last.MapId == mousePos.MapId
            && (last.Position - mousePos.Position).LengthSquared() < 0.01f
            && _updateAccumulator < UpdateInterval)
        {
            return;
        }

        _updateAccumulator = 0f;
        _lastMousePos = mousePos;
        RaisePredictiveEvent(new LaserBeamEndpointPositionEvent(GetNetEntity(player), mousePos));
    }
}
