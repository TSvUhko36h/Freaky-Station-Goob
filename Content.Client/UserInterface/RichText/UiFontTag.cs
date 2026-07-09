using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface.RichText;
using Robust.Shared.IoC;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Client.UserInterface.RichText;

/// <summary>
/// Overrides engine <see cref="FontTag"/> to respect the active UI font style.
/// </summary>
public sealed class UiFontTag : IMarkupTagHandler
{
    [Dependency] private readonly IResourceCache _resourceCache = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

    public string Name => "font";

    public void PushDrawContext(MarkupNode node, MarkupDrawingContext context)
    {
        var fontId = node.Value.StringValue ?? FontTag.DefaultFont;
        var font = UiMarkupFontHelper.ResolveFont(context.Font, node, _resourceCache, _prototypeManager, fontId);
        context.Font.Push(font);
    }

    public void PopDrawContext(MarkupNode node, MarkupDrawingContext context)
    {
        context.Font.Pop();
    }
}
