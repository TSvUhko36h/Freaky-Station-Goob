using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface.RichText;
using Robust.Shared.IoC;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Client.UserInterface.RichText;

/// <summary>
/// Overrides engine <see cref="ItalicTag"/> to respect the active UI font style.
/// </summary>
public sealed class UiItalicTag : IMarkupTagHandler
{
    [Dependency] private readonly IResourceCache _resourceCache = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

    public string Name => "italic";

    public void PushDrawContext(MarkupNode node, MarkupDrawingContext context)
    {
        var font = UiMarkupFontHelper.ResolveFont(
            context.Font,
            node,
            _resourceCache,
            _prototypeManager,
            ItalicTag.ItalicFont);

        context.Font.Push(font);
    }

    public void PopDrawContext(MarkupNode node, MarkupDrawingContext context)
    {
        context.Font.Pop();
    }
}
