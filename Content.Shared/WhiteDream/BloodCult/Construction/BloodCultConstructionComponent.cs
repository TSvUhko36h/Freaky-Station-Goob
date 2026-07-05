using Content.Shared._White.RadialSelector;
using Robust.Shared.GameStates;

namespace Content.Shared.WhiteDream.BloodCult.Construction;

[RegisterComponent, NetworkedComponent]
public sealed partial class BloodCultConstructionComponent : Component
{
    [DataField(required: true)]
    public List<RadialSelectorEntry> Entries = new();
}
