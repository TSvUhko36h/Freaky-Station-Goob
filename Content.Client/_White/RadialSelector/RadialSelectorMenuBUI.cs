using Content.Client.UserInterface.Controls;
using Content.Shared._White.RadialSelector;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.Input;

// ReSharper disable InconsistentNaming

namespace Content.Client._White.RadialSelector;

[UsedImplicitly]
public sealed class RadialSelectorMenuBUI(EntityUid owner, Enum uiKey)
    : RadialSelectorMenuUiBase(owner, uiKey)
{
    [Dependency] private readonly IClyde _displayManager = default!;
    [Dependency] private readonly IInputManager _inputManager = default!;

    private readonly RadialMenu _menu = new()
    {
        HorizontalExpand = true,
        VerticalExpand = true,
        BackButtonStyleClass = "RadialMenuBackButton",
        CloseButtonStyleClass = "RadialMenuCloseButton"
    };

    private bool _openCentered;

    protected override void Open()
    {
        base.Open();

        _menu.OnClose += Close;
        OnEntrySelected = _menu.Close;

        ApplyState(State);
        OpenMenuAtPosition();
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);
        ApplyState(state);
        OpenMenuAtPosition();
    }

    private void ApplyState(BoundUserInterfaceState? state)
    {
        if (state is not RadialSelectorState radialSelectorState)
            return;

        ClearExistingContainers(_menu);
        CreateMenu(radialSelectorState.Entries, _menu);
        _openCentered = radialSelectorState.OpenCentered;
    }

    private void OpenMenuAtPosition()
    {
        if (_openCentered)
            _menu.OpenCentered();
        else
            _menu.OpenCenteredAt(_inputManager.MouseScreenPosition.Position / _displayManager.ScreenSize);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
            _menu.Dispose();
    }
}
