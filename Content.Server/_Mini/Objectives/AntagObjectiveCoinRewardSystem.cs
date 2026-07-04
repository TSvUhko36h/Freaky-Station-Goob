using Content.Server._Mini.AntagTokens;
using Content.Server.Popups;
using Content.Server.Roles;
using Content.Shared.GameTicking;
using Content.Shared.Mind;
using Content.Shared.Objectives.Systems;
using Robust.Server.Player;
using Robust.Shared.Player;
using Robust.Shared.Timing;

namespace Content.Server._Mini.Objectives;

/// <summary>
/// Grants one antag token when a player completes every antagonist objective in a round.
/// </summary>
public sealed class AntagObjectiveCoinRewardSystem : EntitySystem
{
    private const int RewardCoins = 1;
    private const float CheckInterval = 2f;

    [Dependency] private readonly AntagTokenSystem _antagTokens = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly RoleSystem _roles = default!;
    [Dependency] private readonly SharedObjectivesSystem _objectives = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    private float _accumulator;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RoundRestartCleanupEvent>(OnRoundRestart);
    }

    private void OnRoundRestart(RoundRestartCleanupEvent _)
    {
        _accumulator = 0f;
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        _accumulator += frameTime;
        if (_accumulator < CheckInterval)
            return;

        _accumulator = 0f;

        var query = EntityQueryEnumerator<MindComponent>();
        while (query.MoveNext(out var mindId, out var mind))
        {
            if (mind.Objectives.Count == 0)
                continue;

            if (HasComp<AntagObjectiveCoinRewardComponent>(mindId))
                continue;

            if (!_roles.MindIsAntagonist(mindId))
                continue;

            if (!AllObjectivesComplete(mindId, mind))
                continue;

            TryGrantReward(mindId, mind);
        }
    }

    public bool IsRewardGranted(EntityUid mindId)
    {
        return HasComp<AntagObjectiveCoinRewardComponent>(mindId);
    }

    public bool AreAllObjectivesComplete(EntityUid mindId, MindComponent mind)
    {
        if (mind.Objectives.Count == 0 || !_roles.MindIsAntagonist(mindId))
            return false;

        return AllObjectivesComplete(mindId, mind);
    }

    private bool AllObjectivesComplete(EntityUid mindId, MindComponent mind)
    {
        foreach (var objective in mind.Objectives)
        {
            if (!_objectives.IsCompleted(objective, (mindId, mind)))
                return false;
        }

        return true;
    }

    private void TryGrantReward(EntityUid mindId, MindComponent mind)
    {
        if (mind.UserId is not { } userId || !_playerManager.TryGetSessionById(userId, out var session))
            return;

        EnsureComp<AntagObjectiveCoinRewardComponent>(mindId);

        if (!_antagTokens.AddBalance(session.UserId, RewardCoins, out var granted, out _))
            granted = 0;

        var message = Loc.GetString("antag-objective-reward-popup", ("amount", granted > 0 ? granted : RewardCoins));
        var popupEntity = mind.CurrentEntity ?? session.AttachedEntity;
        if (popupEntity is { } target)
            _popup.PopupEntity(message, target, session);
    }
}
