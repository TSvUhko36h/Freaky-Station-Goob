from pathlib import Path

rarity_order = ["rare", "superRare", "epic", "mythic", "legendary"]
rarity_ru = {
    "rare": "Редкий",
    "superRare": "Сверхредкий",
    "epic": "Эпический",
    "mythic": "Мифический",
    "legendary": "Легендарный",
}
rarity_en = {
    "rare": "Rare",
    "superRare": "Super Rare",
    "epic": "Epic",
    "mythic": "Mythic",
    "legendary": "Legendary",
}

# key -> (ru_name, ru_desc_template, en_name, en_desc_template)
# {count} for targets, {minutes} for survive

templates = {
    "cargo-bounty": ("Грузовой заказ", "Зайдите за грузчика и оформите {$count} грузовых заказа на консоли.", "Cargo order", "Play cargo technician and print {$count} cargo bounties."),
    "typan-cargo-bounty": ("Груз Тайпана", "Зайдите за грузчика Тайпана и оформите {$count} грузовых заказа.", "Taipan cargo", "Play Taipan cargo tech and print {$count} cargo bounties."),
    "heal-general": ("Первая помощь", "Вылечите {$count} других игроков на любой роли.", "First aid", "Heal {$count} other players on any role."),
    "heal-medic": ("Дежурный врач", "Зайдите за врача и вылечите {$count} игроков.", "Duty doctor", "Play medical doctor and heal {$count} players."),
    "heal-typan-medic": ("Медик Тайпана", "Зайдите за медика Тайпана и вылечите {$count} игроков.", "Taipan medic", "Play Taipan medic and heal {$count} players."),
    "heal-paramedic": ("Полевая помощь", "Зайдите за парамедика и вылечите {$count} игроков.", "Field care", "Play paramedic and heal {$count} players."),
    "heal-cmo": ("Главврач", "Зайдите за главврача и вылечите {$count} игроков.", "Chief physician", "Play CMO and heal {$count} players."),
    "stamp-hop": ("Печать ГП", "Зайдите за главу персонала и поставьте печать {$count} раза.", "HoP stamp", "Play Head of Personnel and stamp documents {$count} times."),
    "stamp-typan-commander": ("Печать командира", "Зайдите за командира Тайпана и поставьте печать {$count} раза.", "Commander stamp", "Play Taipan commander and stamp {$count} times."),
    "mining-salvage": ("Шахтёр", "Зайдите за утилизатора и заработайте {$count} очков утилизации за раунд.", "Miner", "Play salvage specialist and earn {$count} mining points in one round."),
    "mining-typan-cargo": ("Утиль Тайпана", "Зайдите за грузчика Тайпана и заработайте {$count} очков утилизации.", "Taipan salvage", "Play Taipan cargo tech and earn {$count} mining points."),
    "cuff-security": ("Задержание", "Зайдите за офицера СБ и наденьте наручники на {$count} человек.", "Detention", "Play security officer and cuff {$count} people."),
    "cuff-typan-patrol": ("Патрульный арест", "Зайдите за патрульного Тайпана и наденьте наручники на {$count} человек.", "Patrol arrest", "Play Taipan patrol and cuff {$count} people."),
    "unlock-scientist": ("Исследование", "Зайдите за учёного и откройте {$count} технологию.", "Research", "Play scientist and unlock {$count} technology."),
    "unlock-typan-science": ("Наука Тайпана", "Зайдите за учёного Тайпана и откройте {$count} технологию.", "Taipan science", "Play Taipan scientist and unlock {$count} technology."),
    "cook-chef": ("Кухня", "Зайдите за повара и приготовьте {$count} блюда.", "Kitchen", "Play chef and cook {$count} meals."),
    "cook-typan-chef": ("Кухня Тайпана", "Зайдите за повара Тайпана и приготовьте {$count} блюда.", "Taipan kitchen", "Play Taipan chef and cook {$count} meals."),
    "harvest-botanist": ("Теплица", "Зайдите за ботаника и соберите урожай {$count} раз.", "Greenhouse", "Play botanist and harvest {$count} times."),
    "harvest-typan-botanist": ("Урожай Тайпана", "Зайдите за ботаника Тайпана и соберите урожай {$count} раз.", "Taipan harvest", "Play Taipan botanist and harvest {$count} times."),
    "weld-engineer": ("Сварка", "Зайдите за инженера и выполните {$count} сварочных работ.", "Welding", "Play station engineer and complete {$count} welds."),
    "weld-typan-atmos": ("Сварка Тайпана", "Зайдите за атмосферника Тайпана и выполните {$count} сварочных работ.", "Taipan welding", "Play Taipan atmos tech and complete {$count} welds."),
    "clean-janitor": ("Уборка", "Зайдите за уборщика и очистите {$count} пятен.", "Cleaning", "Play janitor and clean {$count} decals."),
    "clean-typan-patrol": ("Чистый сектор", "Зайдите за патрульного Тайпана и очистите {$count} пятен.", "Clean sector", "Play Taipan patrol and clean {$count} decals."),
    "inject-paramedic": ("Инъекции", "Зайдите за парамедика и сделайте {$count} инъекций.", "Injections", "Play paramedic and inject {$count} players."),
    "inject-typan-medic": ("Уколы Тайпана", "Зайдите за медика Тайпана и сделайте {$count} инъекций.", "Taipan injections", "Play Taipan medic and inject {$count} players."),
    "inject-chemist": ("Химикат", "Зайдите за химика и сделайте {$count} инъекций.", "Chemistry", "Play chemist and inject {$count} players."),
    "kill-security": ("Зачистка", "Зайдите за офицера СБ и устраните {$count} враждебных существ.", "Clearance", "Play security officer and kill {$count} hostiles."),
    "kill-typan-patrol": ("Охота Тайпана", "Зайдите за патрульного Тайпана и устраните {$count} враждебных существ.", "Taipan hunt", "Play Taipan patrol and kill {$count} hostiles."),
    "scan-detective": ("Следствие", "Зайдите за детектива и проведите {$count} сканирования.", "Investigation", "Play detective and scan {$count} times."),
    "lathe-scientist": ("Лате", "Зайдите за учёного и изготовьте {$count} предметов на лате.", "Lathe", "Play scientist and produce {$count} items on a lathe."),
    "lathe-typan-science": ("Производство Тайпана", "Зайдите за учёного Тайпана и изготовьте {$count} предметов на лате.", "Taipan lathe", "Play Taipan scientist and produce {$count} lathe items."),
    "repair-engineer": ("Ремонт", "Зайдите за инженера и отремонтируйте {$count} объекта.", "Repair", "Play station engineer and repair {$count} objects."),
    "repair-typan-atmos": ("Ремонт Тайпана", "Зайдите за атмосферника Тайпана и отремонтируйте {$count} объекта.", "Taipan repair", "Play Taipan atmos tech and repair {$count} objects."),
    "emote-general": ("Эмоции", "Используйте {$count} эмоций за раунд на любой роли.", "Emotes", "Use {$count} emotes in one round on any role."),
    "emote-mime": ("Мим", "Зайдите за мима и используйте {$count} эмоций.", "Mime", "Play mime and use {$count} emotes."),
    "emote-clown": ("Клоун", "Зайдите за клоуна и используйте {$count} эмоций.", "Clown", "Play clown and use {$count} emotes."),
    "survive-general": ("Смена", "Отыграйте любую роль не менее {$minutes} минут за раунд.", "Shift", "Play any role for at least {$minutes} minutes in one round."),
    "survive-captain": ("Капитан", "Зайдите за капитана и продержитесь в живых не менее {$minutes} минут.", "Captain", "Play captain and stay alive for at least {$minutes} minutes."),
    "pacifist": ("Пацифист", "За раунд не ударьте ни одного игрока. Нужно отыграть не менее {$minutes} минут живым.", "Pacifist", "Do not hit any player during the round. Requires {$minutes} minutes alive."),
    "untouchable": ("Неуязвимый", "За раунд не получите урона. Нужно отыграть не менее {$minutes} минут живым.", "Untouchable", "Take no damage during the round. Requires {$minutes} minutes alive."),
}

# Legendary bank overrides
legendary_bank_ru = "Зайдите за грузчика и доведите суммарный баланс счетов станции до {$count} кредитов."
legendary_bank_en = "Play cargo technician and raise total station account balance to {$count} credits."
legendary_bank_typan_ru = "Зайдите за грузчика Тайпана и доведите суммарный баланс счетов станции до {$count} кредитов."
legendary_bank_typan_en = "Play Taipan cargo tech and raise total station balance to {$count} credits."

families = [k for k in templates]

def rarity_prefix(ry, ru_name):
    tag = rarity_ru[ry] if ru_name else rarity_en[ry]
    return f"[{tag}] "

ru_lines = [
    "# SPDX-FileCopyrightText: 2026 Casha",
    "",
    "daily-quest-section-title = Ежедневные квесты",
    "daily-quest-section-summary = Ежедневные квесты ({$done}/{$total} получено)",
    "daily-quest-empty = Сегодня квесты ещё не назначены. Зайдите в игру, чтобы получить задания.",
    "daily-quest-role-hint = Роль: {$role}",
    "daily-quest-progress = {$current} / {$target}",
    "daily-quest-status-active = В процессе",
    "daily-quest-status-complete = Выполнено — награда начислена!",
    "daily-quest-status-claimed = Получено +{$amount} монет",
    "daily-quest-claim = Забрать награду",
    "daily-quest-claim-success = Квест выполнен! Получено монет: {$amount}.",
    "daily-quest-claim-empty = Награда по квесту не начислена.",
    "daily-quest-replace = Заменить задание",
    "daily-quest-replace-pending = Замена задания...",
    "daily-quest-replace-denied = Сейчас этот квест нельзя заменить.",
    "daily-quest-replace-denied-loading = Данные квестов ещё загружаются. Попробуйте через несколько секунд.",
    "daily-quest-replace-denied-progress = Заменить можно только задание без прогресса.",
    "daily-quest-replace-denied-used = Замену заданий можно использовать только один раз в день.",
    "daily-quest-replace-used-today = Замена заданий на сегодня уже использована.",
    "daily-quest-replace-empty-pool = Сегодня больше нет доступных квестов для замены.",
    "daily-quest-replace-success = Задание заменено.",
    "",
    "daily-quest-rarity-rare = Редкий",
    "daily-quest-rarity-super-rare = Сверхредкий",
    "daily-quest-rarity-epic = Эпический",
    "daily-quest-rarity-mythic = Мифический",
    "daily-quest-rarity-legendary = Легендарный",
    "",
]
en_lines = [
    "# SPDX-FileCopyrightText: 2026 Casha",
    "",
    "daily-quest-section-title = Daily quests",
    "daily-quest-section-summary = Daily quests ({$done}/{$total} claimed)",
    "daily-quest-empty = No quests assigned yet today. Join a round to receive assignments.",
    "daily-quest-role-hint = Role: {$role}",
    "daily-quest-progress = {$current} / {$target}",
    "daily-quest-status-active = In progress",
    "daily-quest-status-complete = Complete — reward granted!",
    "daily-quest-status-claimed = Claimed +{$amount} coins",
    "daily-quest-claim = Claim reward",
    "daily-quest-claim-success = Quest complete! Coins received: {$amount}.",
    "daily-quest-claim-empty = No quest reward was granted.",
    "daily-quest-replace = Replace quest",
    "daily-quest-replace-pending = Replacing quest...",
    "daily-quest-replace-denied = This quest cannot be replaced right now.",
    "daily-quest-replace-denied-loading = Quest data is still loading. Try again in a few seconds.",
    "daily-quest-replace-denied-progress = Only quests with no progress can be replaced.",
    "daily-quest-replace-denied-used = Quest replacement can only be used once per day.",
    "daily-quest-replace-used-today = Today's quest replacement has already been used.",
    "daily-quest-replace-empty-pool = No more quests are available for replacement today.",
    "daily-quest-replace-success = Quest replaced.",
    "",
    "daily-quest-rarity-rare = Rare",
    "daily-quest-rarity-super-rare = Super Rare",
    "daily-quest-rarity-epic = Epic",
    "daily-quest-rarity-mythic = Mythic",
    "daily-quest-rarity-legendary = Legendary",
    "",
]

for key, (ru_name, ru_desc, en_name, en_desc) in templates.items():
    for ry in rarity_order:
        ru_lines.append(f"daily-quest-{key}-{ry}-name = {ru_name}")
        en_lines.append(f"daily-quest-{key}-{ry}-name = {en_name}")
        if key == "cargo-bounty" and ry == "legendary":
            ru_lines.append(f"daily-quest-{key}-{ry}-desc = {legendary_bank_ru}")
            en_lines.append(f"daily-quest-{key}-{ry}-desc = {legendary_bank_en}")
        elif key == "typan-cargo-bounty" and ry == "legendary":
            ru_lines.append(f"daily-quest-{key}-{ry}-desc = {legendary_bank_typan_ru}")
            en_lines.append(f"daily-quest-{key}-{ry}-desc = {legendary_bank_typan_en}")
        else:
            ru_lines.append(f"daily-quest-{key}-{ry}-desc = {ru_desc}")
            en_lines.append(f"daily-quest-{key}-{ry}-desc = {en_desc}")
        ru_lines.append("")
        en_lines.append("")

Path("Resources/Locale/ru-RU/_Goobstation/daily-quests.ftl").write_text("\n".join(ru_lines), encoding="utf-8")
Path("Resources/Locale/en-US/_Goobstation/daily-quests.ftl").write_text("\n".join(en_lines), encoding="utf-8")
print("locale done")
