using Robust.Shared.Prototypes;

[Prototype("adminRankIcon")]
public sealed class AdminRankIconPrototype : IPrototype
{
    [IdDataField] public string ID { get; } = string.Empty;
    
    [DataField(required: true)] public string Rank { get; set; } = string.Empty;
    [DataField(required: true)] public string Icon { get; set; } = string.Empty;
}