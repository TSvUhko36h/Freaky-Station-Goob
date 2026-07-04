using System;
using Content.Client.UserInterface.RichText;
using Robust.Client.UserInterface.RichText;

namespace Content.Client.UserInterface.RichText;

public static class UiMarkupTags
{
    public static readonly Type[] NewsFormattingTags =
    [
        typeof(UiBoldItalicTag),
        typeof(UiBoldTag),
        typeof(BulletTag),
        typeof(ColorTag),
        typeof(HeadingTag),
        typeof(UiItalicTag),
        typeof(MonoTag),
    ];
}
