# Temporary generator for tiered daily quests
from pathlib import Path

rarity_order = ["Rare", "SuperRare", "Epic", "Mythic", "Legendary"]
rarity_yaml = {
    "Rare": "rare",
    "SuperRare": "superRare",
    "Epic": "epic",
    "Mythic": "mythic",
    "Legendary": "legendary",
}

heal_sprite = """  sprite:
    sprite: /Textures/Interface/Misc/health_icons.rsi
    state: Fine"""

families = [
    ("CargoBounty", "FulfillCargoBounty", "CargoTechnician", [2, 4, 6, 10, None], "cargo-bounty", "cargo-bounty", {"legendary_type": "EarnStationBankBalance", "legendary_target": 100000}),
    ("TypanCargoBounty", "FulfillCargoBounty", "TypanCargotech", [2, 4, 6, 10, None], "typan-cargo-bounty", "typan-cargo-bounty", {"legendary_type": "EarnStationBankBalance", "legendary_target": 80000}),
    ("HealGeneral", "HealOthers", None, [3, 5, 8, 12, 20], "heal-general", "heal-general", {"heal_icon": True}),
    ("HealMedic", "HealOthers", "MedicalDoctor", [4, 6, 10, 15, 25], "heal-medic", "heal-medic", {"heal_icon": True}),
    ("HealTypanMedic", "HealOthers", "TypanMedic", [5, 8, 12, 18, 30], "heal-typan-medic", "heal-typan-medic", {"heal_icon": True}),
    ("HealParamedic", "HealOthers", "Paramedic", [4, 6, 9, 14, 22], "heal-paramedic", "heal-paramedic", {"heal_icon": True}),
    ("HealCmo", "HealOthers", "ChiefMedicalOfficer", [5, 8, 12, 18, 28], "heal-cmo", "heal-cmo", {"heal_icon": True}),
    ("StampHop", "StampDocuments", "HeadOfPersonnel", [2, 4, 6, 8, 12], "stamp-hop", "stamp-hop", {}),
    ("StampTypanCommander", "StampDocuments", "TypanCommander", [2, 4, 6, 9, 14], "stamp-typan-commander", "stamp-typan-commander", {}),
    ("MiningSalvage", "EarnMiningPoints", "SalvageSpecialist", [1500, 3000, 5000, 8000, 15000], "mining-salvage", "mining-salvage", {}),
    ("MiningTypanCargo", "EarnMiningPoints", "TypanCargotech", [2000, 4000, 6500, 10000, 18000], "mining-typan-cargo", "mining-typan-cargo", {}),
    ("CuffSecurity", "CuffPlayers", "SecurityOfficer", [2, 3, 5, 7, 10], "cuff-security", "cuff-security", {}),
    ("CuffTypanPatrol", "CuffPlayers", "TypanPatrol", [2, 3, 5, 7, 10], "cuff-typan-patrol", "cuff-typan-patrol", {}),
    ("UnlockScientist", "UnlockTechnology", "Scientist", [1, 2, 3, 4, 5], "unlock-scientist", "unlock-scientist", {}),
    ("UnlockTypanScience", "UnlockTechnology", "TypanScience", [1, 2, 3, 4, 6], "unlock-typan-science", "unlock-typan-science", {}),
    ("CookChef", "CookMeals", "Chef", [3, 5, 8, 12, 16], "cook-chef", "cook-chef", {}),
    ("CookTypanChef", "CookMeals", "TypanChef", [3, 5, 8, 12, 16], "cook-typan-chef", "cook-typan-chef", {}),
    ("HarvestBotanist", "HarvestPlants", "Botanist", [8, 12, 18, 25, 40], "harvest-botanist", "harvest-botanist", {}),
    ("HarvestTypanBotanist", "HarvestPlants", "TypanBotanist", [10, 15, 22, 30, 45], "harvest-typan-botanist", "harvest-typan-botanist", {}),
    ("WeldEngineer", "WeldStructures", "StationEngineer", [5, 8, 12, 18, 25], "weld-engineer", "weld-engineer", {}),
    ("WeldTypanAtmos", "WeldStructures", "TypanAtmosTech", [5, 8, 12, 18, 25], "weld-typan-atmos", "weld-typan-atmos", {}),
    ("CleanJanitor", "CleanDecals", "Janitor", [15, 25, 35, 50, 70], "clean-janitor", "clean-janitor", {}),
    ("CleanTypanPatrol", "CleanDecals", "TypanPatrol", [12, 20, 30, 45, 60], "clean-typan-patrol", "clean-typan-patrol", {}),
    ("InjectParamedic", "InjectPatients", "Paramedic", [4, 6, 9, 12, 18], "inject-paramedic", "inject-paramedic", {}),
    ("InjectTypanMedic", "InjectPatients", "TypanMedic", [3, 5, 8, 12, 18], "inject-typan-medic", "inject-typan-medic", {}),
    ("InjectChemist", "InjectPatients", "Chemist", [5, 8, 12, 16, 24], "inject-chemist", "inject-chemist", {}),
    ("KillSecurity", "KillHostiles", "SecurityOfficer", [3, 5, 8, 12, 20], "kill-security", "kill-security", {}),
    ("KillTypanPatrol", "KillHostiles", "TypanPatrol", [5, 8, 12, 18, 25], "kill-typan-patrol", "kill-typan-patrol", {}),
    ("ScanDetective", "ScanForensics", "Detective", [3, 5, 7, 10, 15], "scan-detective", "scan-detective", {}),
    ("LatheScientist", "LatheProduce", "Scientist", [5, 8, 12, 18, 25], "lathe-scientist", "lathe-scientist", {}),
    ("LatheTypanScience", "LatheProduce", "TypanScience", [5, 8, 12, 18, 25], "lathe-typan-science", "lathe-typan-science", {}),
    ("RepairEngineer", "RepairStructures", "StationEngineer", [3, 5, 8, 12, 18], "repair-engineer", "repair-engineer", {}),
    ("RepairTypanAtmos", "RepairStructures", "TypanAtmosTech", [3, 5, 8, 12, 18], "repair-typan-atmos", "repair-typan-atmos", {}),
    ("EmoteGeneral", "PerformEmotes", None, [6, 10, 15, 22, 30], "emote-general", "emote-general", {"icon": "JobIconNoId"}),
    ("EmoteMime", "PerformEmotes", "Mime", [8, 12, 18, 25, 35], "emote-mime", "emote-mime", {}),
    ("EmoteClown", "PerformEmotes", "Clown", [10, 15, 22, 30, 40], "emote-clown", "emote-clown", {}),
    ("SurviveGeneral", "SurviveRound", None, [1200, 1500, 1800, 2400, 3600], "survive-general", "survive-general", {"icon": "JobIconNoId", "time_based": True}),
    ("SurviveCaptain", "SurviveRound", "Captain", [1800, 2100, 2400, 3000, 4200], "survive-captain", "survive-captain", {"time_based": True}),
    ("Pacifist", "NoMeleeHits", None, [1, 1, 1, 1, 1], "pacifist", "pacifist", {"icon": "JobIconMime", "time_based": True, "min_playtime": [900, 1200, 1500, 1800, 2400]}),
    ("Untouchable", "NoDamageTaken", None, [1, 1, 1, 1, 1], "untouchable", "untouchable", {"icon": "JobIconMedicalDoctor", "time_based": True, "min_playtime": [900, 1200, 1500, 1800, 2400]}),
]

lines = [
    "# SPDX-FileCopyrightText: 2026 Casha",
    "# Daily quest pool with Brawl Stars-style rarity tiers.",
    "",
]

for prefix, qtype, job, targets, nkey, _dkey, extra in families:
    for i, rarity in enumerate(rarity_order):
        rid = f"{prefix}{rarity}"
        ry = rarity_yaml[rarity]
        if rarity == "Legendary" and extra.get("legendary_type"):
            quest_type = extra["legendary_type"]
            target = extra["legendary_target"]
        else:
            quest_type = qtype
            target = targets[i]

        lines.append("- type: dailyQuest")
        lines.append(f"  id: {rid}")
        lines.append(f"  rarity: {ry}")
        lines.append(f"  questType: {quest_type}")
        if extra.get("heal_icon"):
            lines.append(heal_sprite)
        if extra.get("icon"):
            lines.append(f"  icon: {extra['icon']}")
        lines.append(f"  name: daily-quest-{nkey}-{ry}-name")
        lines.append(f"  description: daily-quest-{nkey}-{ry}-desc")
        if job:
            lines.append(f"  requiredJob: {job}")
        if extra.get("time_based") and quest_type == "SurviveRound":
            lines.append("  targetCount: 1")
            lines.append(f"  minRoundPlaytime: {target}")
        elif quest_type in ("NoMeleeHits", "NoDamageTaken"):
            lines.append("  targetCount: 1")
            min_pt = extra.get("min_playtime", 900)
            if isinstance(min_pt, list):
                min_pt = min_pt[i]
            lines.append(f"  minRoundPlaytime: {min_pt}")
        else:
            lines.append(f"  targetCount: {target}")
        lines.append("")
    lines.append("")

out = Path("Resources/Prototypes/_Mini/DailyQuests/daily_quests.yml")
out.write_text("\n".join(lines), encoding="utf-8")
print(f"Wrote {len(families) * 5} quests to {out}")
