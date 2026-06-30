// SPDX-FileCopyrightText: 2026 Casha
// Мини-станция/Freaky-station, Licensed under custom terms with restrictions on public hosting and commercial use, full text: https://raw.githubusercontent.com/ministation/mini-station-goob/master/LICENSE.TXT

using System.Collections.Generic;
using Content.Shared.Roles;

namespace Content.Shared._Mini.JobUnlock;

/// <summary>
/// Default unlock costs for lobby jobs (antag tokens).
/// </summary>
public static class JobUnlockPricing
{
    public const int CaptainCost = 50;
    public const int HeadCost = 40;
    /// <summary>Офицер синего щита.</summary>
    public const int BlueshieldOfficerCost = 100;
    public const int RegularLowCost = 10;
    public const int RegularMidCost = 15;
    public const int RegularHighCost = 20;

    private static readonly HashSet<string> Heads =
    [
        "HeadOfSecurity",
        "ChiefMedicalOfficer",
        "ChiefEngineer",
        "ResearchDirector",
        "Quartermaster",
        "HeadOfPersonnel",
        "NanotrasenRepresentative",
    ];

    public static bool TryGetDefaultCost(JobPrototype job, out int cost)
    {
        cost = 0;

        if (!job.SetPreference)
            return false;

        cost = job.ID switch
        {
            "Captain" => CaptainCost,
            "BlueshieldOfficer" => BlueshieldOfficerCost,
            _ when Heads.Contains(job.ID) => HeadCost,
            "Warden" or "Detective" or "SecurityOfficer" => RegularHighCost,
            _ => RegularCost(job),
        };

        return true;
    }

    private static int RegularCost(JobPrototype job)
    {
        if (job.RealDisplayWeight >= 15)
            return RegularHighCost;

        if (job.RealDisplayWeight >= 5)
            return RegularMidCost;

        return RegularLowCost;
    }
}
