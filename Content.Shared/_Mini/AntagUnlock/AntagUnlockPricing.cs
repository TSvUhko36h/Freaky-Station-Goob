// SPDX-FileCopyrightText: 2026 Casha
// Мини-станция/Freaky-station, Licensed under custom terms with restrictions on public hosting and commercial use, full text: https://raw.githubusercontent.com/ministation/mini-station-goob/master/LICENSE.TXT

using Content.Shared.Roles;

namespace Content.Shared._Mini.AntagUnlock;

public static class AntagUnlockPricing
{
    public const int DefaultCost = 15;
    public const int ShopCostMultiplier = 5;
    public const int MinCost = 10;

    public static int ResolveCost(AntagPrototype antag, int? shopTokenCost)
    {
        if (shopTokenCost is int cost && cost > 0)
            return Math.Max(MinCost, cost * ShopCostMultiplier);

        return DefaultCost;
    }
}
