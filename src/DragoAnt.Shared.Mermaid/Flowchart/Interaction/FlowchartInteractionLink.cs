using DragoAnt.Shared.Text;

namespace DragoAnt.Shared.Mermaid.Flowchart.Interaction;

public sealed class FlowchartInteractionLink : FlowchartInteraction
{
    private readonly FlowchartInteractionLinkOpenType _openType;

    /// <inheritdoc />
    public FlowchartInteractionLink(string link, string? toolTip = null,
        FlowchartInteractionLinkOpenType openType = FlowchartInteractionLinkOpenType.Blank)
        : base(_ => GetReference(link), toolTip)
    {
        _openType = openType;
    }

    private static string GetReference(string reference)
    {
        return $"\"{reference}\"";
    }

    /// <inheritdoc />
    protected internal override bool Print(AdvStringBuilder builder, MermaidPrintConfig config)
    {
        var ret = base.Print(builder, config);
        if (!ret)
        {
            return ret;
        }
        var openType = _openType switch
        {
            FlowchartInteractionLinkOpenType.Default => null,
            FlowchartInteractionLinkOpenType.Self => "_self",
            FlowchartInteractionLinkOpenType.Blank => "_blank",
            FlowchartInteractionLinkOpenType.Parent => "_parent",
            FlowchartInteractionLinkOpenType.Top => "_top",
            _ => throw new ArgumentOutOfRangeException()
        };
        if (openType is not null)
        {
            builder.Append(' ');
            builder.Append(openType);
        }
        return true;
    }
}