using Robust.Client.Graphics;

namespace Content.Client.UserInterface.RichText;

/// <summary>
/// Resolves markup font prototype ids to runtime font stacks for the active UI font style.
/// </summary>
public interface IMarkupFontResolver
{
    Font ResolveFont(string fontId, int size);
}
