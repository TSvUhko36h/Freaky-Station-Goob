// SPDX-FileCopyrightText: 2025 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Numerics;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Shared.Xenobiology.Components;

/// <summary>
/// Marks a slime currently playing a merge animation and absorbing nearby slimes.
/// </summary>
[RegisterComponent]
public sealed partial class SlimeMergingComponent : Component
{
    public EntityUid Anchor;

    public bool IsAnchor;

    public Vector2 MergeStartPosition;

    public TimeSpan StartedAt;

    public TimeSpan Duration;

    public int TotalCount;

    public ProtoId<BreedPrototype> Breed;

    public float StartScale = 1f;

    public float TargetScale = 1f;

    public List<EntityUid> Victims = [];
}
