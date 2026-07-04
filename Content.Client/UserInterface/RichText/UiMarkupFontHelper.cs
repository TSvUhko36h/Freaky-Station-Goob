using System.Collections.Generic;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface.RichText;
using Robust.Shared.IoC;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Client.UserInterface.RichText;

internal static class UiMarkupFontHelper
{
    public static Font ResolveFont(
        Stack<Font> contextFontStack,
        MarkupNode node,
        IResourceCache cache,
        IPrototypeManager prototypeManager,
        string fontId)
    {
        var size = FontTag.DefaultSize;

        if (contextFontStack.TryPeek(out var previousFont))
        {
            switch (previousFont)
            {
                case VectorFont vectorFont:
                    size = vectorFont.Size;
                    break;
                case StackedFont stackedFont:
                    if (stackedFont.Stack.Length == 0 || stackedFont.Stack[0] is not VectorFont stackVectorFont)
                        break;

                    size = stackVectorFont.Size;
                    break;
            }
        }

        if (node.Attributes.TryGetValue("size", out var sizeParameter))
            size = (int) (sizeParameter.LongValue ?? size);

        if (IoCManager.Instance?.TryResolveType<IMarkupFontResolver>(out var resolver) == true)
            return resolver.ResolveFont(fontId, size);

        return FontTag.CreateFont(contextFontStack, node, cache, prototypeManager, fontId);
    }
}
