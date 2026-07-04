using System;
using System.Numerics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Maths;
using Robust.Shared.Timing;

namespace Content.Client.UserInterface;

/// <summary>
/// Single-line label that scrolls horizontally when text overflows its bounds.
/// Uses duplicated text for a seamless loop (end flows into start).
/// </summary>
public sealed class MarqueeLabel : Control
{
    private const float ScrollSpeed = 28f;

    private readonly BoxContainer _scrollRow;
    private readonly Label _labelA;
    private readonly Label _labelB;
    private float _scrollPos;
    private float _loopLength;
    private float _clipWidth;
    private float _textWidth;
    private bool _scrolling;
    private string _text = string.Empty;

    public string Text
    {
        get => _text;
        set
        {
            if (_text == value)
                return;

            _text = value;
            _labelA.Text = value;
            _labelB.Text = value;
            ResetScroll();
            InvalidateMeasure();
            InvalidateArrange();
        }
    }

    public new Color Modulate
    {
        get => _labelA.Modulate;
        set
        {
            _labelA.Modulate = value;
            _labelB.Modulate = value;
        }
    }

    public MarqueeLabel()
    {
        RectClipContent = true;
        MinSize = new Vector2(0, 18);
        MouseFilter = MouseFilterMode.Ignore;

        _scrollRow = new BoxContainer
        {
            Orientation = BoxContainer.LayoutOrientation.Horizontal,
            SeparationOverride = 0,
            VerticalAlignment = VAlignment.Center,
        };

        _labelA = CreateLabel();
        _labelB = CreateLabel();

        _scrollRow.AddChild(_labelA);
        _scrollRow.AddChild(_labelB);
        AddChild(_scrollRow);
    }

    public void SetStyleClass(string styleClass)
    {
        _labelA.StyleClasses.Clear();
        _labelA.StyleClasses.Add(styleClass);
        _labelB.StyleClasses.Clear();
        _labelB.StyleClasses.Add(styleClass);
        InvalidateMeasure();
        InvalidateArrange();
    }

    private static Label CreateLabel()
    {
        return new Label
        {
            HorizontalAlignment = HAlignment.Left,
            VerticalAlignment = VAlignment.Center,
            ClipText = false,
        };
    }

    private void ResetScroll()
    {
        _scrollPos = 0f;
        _loopLength = 0f;
        _textWidth = 0f;
        _scrolling = false;
        _labelB.Visible = false;
    }

    protected override Vector2 MeasureOverride(Vector2 availableSize)
    {
        _labelA.Measure(new Vector2(float.PositiveInfinity, float.PositiveInfinity));
        var labelSize = _labelA.DesiredSize;
        var height = Math.Max(MinSize.Y, labelSize.Y);

        if (HorizontalExpand)
            return new Vector2(0, height);

        var width = availableSize.X > 0 && !float.IsPositiveInfinity(availableSize.X)
            ? availableSize.X
            : labelSize.X;
        return new Vector2(width, height);
    }

    protected override Vector2 ArrangeOverride(Vector2 finalSize)
    {
        _clipWidth = finalSize.X;
        RefreshScrollState(finalSize.Y);
        ApplyScrollPosition(finalSize.Y);
        return finalSize;
    }

    protected override void FrameUpdate(FrameEventArgs args)
    {
        base.FrameUpdate(args);

        if (!IsInsideTree || !_scrolling || _loopLength <= 0f)
            return;

        _scrollPos += ScrollSpeed * args.DeltaSeconds;
        if (_scrollPos >= _loopLength)
            _scrollPos -= _loopLength;

        ApplyScrollPosition(PixelHeight);
    }

    private void RefreshScrollState(float height)
    {
        if (_clipWidth <= 1f)
            return;

        _labelA.Measure(new Vector2(float.PositiveInfinity, height));
        var measuredWidth = _labelA.DesiredPixelSize.X;

        if (measuredWidth - _clipWidth > 1f)
        {
            if (!_scrolling || MathF.Abs(measuredWidth - _textWidth) > 1f)
            {
                _textWidth = measuredWidth;
                _loopLength = _textWidth;
                _scrolling = true;
                _labelB.Visible = true;
            }
        }
        else if (_scrolling)
        {
            ResetScroll();
        }
    }

    private void ApplyScrollPosition(float height)
    {
        _scrollRow.Measure(new Vector2(float.PositiveInfinity, Math.Max(height, MinSize.Y)));
        var rowSize = _scrollRow.DesiredSize;
        _scrollRow.Arrange(UIBox2.FromDimensions(new Vector2(-_scrollPos, 0), rowSize));
    }
}
