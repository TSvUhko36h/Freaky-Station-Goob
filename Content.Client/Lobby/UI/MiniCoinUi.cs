// SPDX-FileCopyrightText: 2026 Casha
// Мини-станция/Freaky-station, Licensed under custom terms with restrictions on public hosting and commercial use, full text: https://raw.githubusercontent.com/ministation/mini-station-goob/master/LICENSE.TXT

using System.Numerics;
using Content.Client.Resources;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Maths;

namespace Content.Client.Lobby.UI;

public static class MiniCoinUi
{
    public const string CoinIconPath = "/Textures/_Mini/Interface/Coin.png";

    public static TextureRect CreateCoinIcon(IResourceCache cache, float scale = 0.4f)
    {
        return new TextureRect
        {
            Texture = cache.GetTexture(CoinIconPath),
            MinSize = new Vector2(16, 16),
            MaxSize = new Vector2(16, 16),
            TextureScale = new Vector2(scale, scale),
            Stretch = TextureRect.StretchMode.KeepCentered,
            VerticalAlignment = Control.VAlignment.Center,
        };
    }
}
