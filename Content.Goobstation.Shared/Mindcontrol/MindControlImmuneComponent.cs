using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Mindcontrol;

/// <summary>
/// Marks an entity as immune to mind control effects.
/// </summary>
[RegisterComponent]
[NetworkedComponent]
public sealed partial class MindControlImmuneComponent : Component;
