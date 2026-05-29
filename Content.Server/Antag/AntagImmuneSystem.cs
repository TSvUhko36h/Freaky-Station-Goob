using Content.Goobstation.Common.Mind;
using Content.Server.Antag.Components;
using Content.Server._CorvaxGoob.Objectives.Components;

namespace Content.Server.Antag;

public sealed class AntagImmuneSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AntagImmuneComponent, GetAntagSelectionBlockerEvent>(OnAntagImmuneBlock);
        SubscribeLocalEvent<AntagObjectiveImmunityComponent, GetAntagSelectionBlockerEvent>(OnObjectiveImmunityBlock);
    }

    private static void OnAntagImmuneBlock(Entity<AntagImmuneComponent> ent, ref GetAntagSelectionBlockerEvent args)
    {
        args.Blocked = true;
    }

    private static void OnObjectiveImmunityBlock(Entity<AntagObjectiveImmunityComponent> ent, ref GetAntagSelectionBlockerEvent args)
    {
        args.Blocked = true;
    }
}
