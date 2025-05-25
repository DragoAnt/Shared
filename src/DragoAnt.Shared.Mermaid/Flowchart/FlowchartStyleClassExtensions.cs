using System.Drawing;

namespace DragoAnt.Shared.Mermaid.Flowchart;

/// <summary>
/// Extensions for simplify set modifiers to <see cref="FlowchartStyleClass"/>
/// </summary>
public static class FlowchartStyleClassExtensions
{
    private static string ToHtml(Color color)
    {
#if NETSTANDARD2_1
        if (color.IsEmpty)
        {
            return string.Empty;
        }
        if (color.IsSystemColor)
        {
            return color.ToKnownColor() switch
            {
                KnownColor.ActiveBorder => "activeborder",
                KnownColor.ActiveCaption => "activecaption",
                KnownColor.GradientActiveCaption => "activecaption",
                KnownColor.ActiveCaptionText => "captiontext",
                KnownColor.AppWorkspace => "appworkspace",
                KnownColor.Control => "buttonface",
                KnownColor.ControlLight => "buttonface",
                KnownColor.ControlDark => "buttonshadow",
                KnownColor.ControlDarkDark => "threeddarkshadow",
                KnownColor.ControlLightLight => "buttonhighlight",
                KnownColor.ControlText => "buttontext",
                KnownColor.Desktop => "background",
                KnownColor.GrayText => "graytext",
                KnownColor.Highlight => "highlight",
                KnownColor.HotTrack => "highlight",
                KnownColor.HighlightText => "highlighttext",
                KnownColor.MenuHighlight => "highlighttext",
                KnownColor.InactiveBorder => "inactiveborder",
                KnownColor.InactiveCaption => "inactivecaption",
                KnownColor.GradientInactiveCaption => "inactivecaption",
                KnownColor.InactiveCaptionText => "inactivecaptiontext",
                KnownColor.Info => "infobackground",
                KnownColor.InfoText => "infotext",
                KnownColor.Menu => "menu",
                KnownColor.MenuBar => "menu",
                KnownColor.MenuText => "menutext",
                KnownColor.ScrollBar => "scrollbar",
                KnownColor.Window => "window",
                KnownColor.WindowFrame => "windowframe",
                KnownColor.WindowText => "windowtext",
                _ => string.Empty
            };
        }

        if (color.IsNamedColor)
        {
            return !(color == Color.LightGray) ? color.Name : "LightGrey";
        }
#endif
#if NETSTANDARD2_0 || NETSTANDARD2_1

        return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
#else
            return ColorTranslator.ToHtml(color);
#endif
    }

    /// <summary>
    /// Sets 'fill' modifier
    /// </summary>
    /// <param name="styleClass"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static FlowchartStyleClass SetFill(this FlowchartStyleClass styleClass, Color color)
    {
        return styleClass.SetFill(ToHtml(color));
    }

    /// <summary>
    /// Sets 'fill' modifier
    /// </summary>
    /// <example>#ccffcc</example>
    /// <param name="styleClass"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static FlowchartStyleClass SetFill(this FlowchartStyleClass styleClass, string? value)
    {
        return styleClass.SetModifier("fill", value);
    }

    /// <summary>
    /// Sets 'stroke-width' modifier
    /// </summary>
    /// <example>2px</example>
    /// <param name="styleClass"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static FlowchartStyleClass SetStrokeWidth(this FlowchartStyleClass styleClass, string? value)
    {
        return styleClass.SetModifier("stroke-width", value);
    }

    /// <summary>
    /// Sets 'stroke-dasharray' modifier
    /// </summary>
    /// <example>2 2</example>
    /// <param name="styleClass"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static FlowchartStyleClass SetStrokeDashArray(this FlowchartStyleClass styleClass, string? value)
    {
        return styleClass.SetModifier("stroke-dasharray", value);
    }

    /// <summary>
    /// Sets 'font-weight' modifier. See more
    /// <a href="https://developer.mozilla.org/en-US/docs/Web/CSS/font-weight">here</a>
    /// </summary>
    /// <example>bold</example>
    /// <param name="styleClass"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static FlowchartStyleClass SetFontWeight(this FlowchartStyleClass styleClass, string? value)
    {
        return styleClass.SetModifier("font-weight", value);
    }

    /// <summary>
    /// Sets 'font-style' modifier. See more
    /// <a href="https://developer.mozilla.org/en-US/docs/Web/CSS/font-style">here</a>
    /// </summary>
    /// <example>italic</example>
    /// <param name="styleClass"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static FlowchartStyleClass SetFontStyle(this FlowchartStyleClass styleClass, string? value)
    {
        return styleClass.SetModifier("font-style", value);
    }

    /// <summary>
    /// Sets 'color' modifier. See more
    /// <a href="https://developer.mozilla.org/en-US/docs/Web/CSS/color">here</a>
    /// </summary>
    /// <example>#ccffcc</example>
    /// <param name="styleClass"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static FlowchartStyleClass SetColor(this FlowchartStyleClass styleClass, Color color)
    {
        return styleClass.SetColor(ToHtml(color));
    }

    /// <summary>
    /// Sets 'color' modifier. See more
    /// <a href="https://developer.mozilla.org/en-US/docs/Web/CSS/color">here</a>
    /// </summary>
    /// <param name="styleClass"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static FlowchartStyleClass SetColor(this FlowchartStyleClass styleClass, string? value)
    {
        return styleClass.SetModifier("color", value);
    }
}