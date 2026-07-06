// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Winkarst <74284083+Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Linq;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Points;
using JetBrains.Annotations;
using Robust.Server.GameStates;
using Robust.Shared.Map;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.GameObjects;
using Robust.Shared.Player;
using Robust.Shared.Utility;

namespace Content.Server.Points;

/// <inheritdoc/>
public sealed class PointSystem : SharedPointSystem
{
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly PvsOverrideSystem _pvsOverride = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PointManagerComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<PointManagerComponent, ComponentShutdown>(OnShutdown);
        SubscribeLocalEvent<ActorComponent, ComponentStartup>(OnActorStartup);
        SubscribeLocalEvent<ActorComponent, EntParentChangedMessage>(OnActorParentChanged);

        _player.PlayerStatusChanged += OnPlayerStatusChanged;
    }

    public override void Shutdown()
    {
        base.Shutdown();
        _player.PlayerStatusChanged -= OnPlayerStatusChanged;
    }

    private void OnPlayerStatusChanged(object? sender, SessionStatusEventArgs e)
    {
        if (e.NewStatus == SessionStatus.InGame)
            UpdatePointManagerOverrides(e.Session);
    }

    private void OnActorStartup(EntityUid uid, ActorComponent component, ComponentStartup args)
    {
        UpdatePointManagerOverrides(component.PlayerSession, uid);
    }

    private void OnActorParentChanged(EntityUid uid, ActorComponent component, EntParentChangedMessage args)
    {
        UpdatePointManagerOverrides(component.PlayerSession, uid);
    }

    private void OnStartup(EntityUid uid, PointManagerComponent component, ComponentStartup args)
    {
        UpdatePointManagerOverrides();
    }

    private void OnShutdown(EntityUid uid, PointManagerComponent component, ComponentShutdown args)
    {
        foreach (var session in _player.Sessions)
        {
            _pvsOverride.RemoveSessionOverride(uid, session);
        }
    }

    private void UpdatePointManagerOverrides(ICommonSession? session = null, EntityUid? player = null)
    {
        var sessions = session != null ? new[] { session } : _player.Sessions;

        foreach (var targetSession in sessions)
        {
            if (targetSession.Status != SessionStatus.InGame)
                continue;

            var attached = player ?? targetSession.AttachedEntity;
            MapId? playerMap = null;

            if (attached != null && TryComp<TransformComponent>(attached, out var xform))
                playerMap = xform.MapID;

            var query = EntityQueryEnumerator<PointManagerComponent, TransformComponent>();
            while (query.MoveNext(out var uid, out _, out var managerXform))
            {
                if (playerMap.HasValue && managerXform.MapID == playerMap.Value)
                    _pvsOverride.AddSessionOverride(uid, targetSession);
                else
                    _pvsOverride.RemoveSessionOverride(uid, targetSession);
            }
        }
    }

    /// <summary>
    /// Adds the specified point value to a player.
    /// </summary>
    [PublicAPI]
    public void AdjustPointValue(EntityUid user, FixedPoint2 value, EntityUid uid, PointManagerComponent? component, ActorComponent? actor = null)
    {
        if (!Resolve(uid, ref component) || !Resolve(user, ref actor, false))
            return;
        AdjustPointValue(actor.PlayerSession.UserId, value, uid, component);
    }

    /// <summary>
    /// Sets the amount of points for a player
    /// </summary>
    [PublicAPI]
    public void SetPointValue(EntityUid user, FixedPoint2 value, EntityUid uid, PointManagerComponent? component, ActorComponent? actor = null)
    {
        if (!Resolve(uid, ref component) || !Resolve(user, ref actor, false))
            return;
        SetPointValue(actor.PlayerSession.UserId, value, uid, component);
    }

    /// <summary>
    /// Gets the amount of points for a given player
    /// </summary>
    [PublicAPI]
    public FixedPoint2 GetPointValue(EntityUid user, EntityUid uid, PointManagerComponent? component, ActorComponent? actor = null)
    {
        if (!Resolve(uid, ref component) || !Resolve(user, ref actor, false))
            return FixedPoint2.Zero;
        return GetPointValue(actor.PlayerSession.UserId, uid, component);
    }

    /// <inheritdoc/>
    public override FormattedMessage GetScoreboard(EntityUid uid, PointManagerComponent? component = null)
    {
        var msg = new FormattedMessage();

        if (!Resolve(uid, ref component))
            return msg;

        var orderedPlayers = component.Points.OrderByDescending(p => p.Value).ToList();
        var place = 1;
        foreach (var (id, points) in orderedPlayers)
        {
            if (!_player.TryGetPlayerData(id, out var data))
                continue;

            msg.AddMarkupOrThrow(Loc.GetString("point-scoreboard-list",
                ("place", place),
                ("name", data.UserName),
                ("points", points.Int())));
            msg.PushNewline();
            place++;
        }

        return msg;
    }
}
