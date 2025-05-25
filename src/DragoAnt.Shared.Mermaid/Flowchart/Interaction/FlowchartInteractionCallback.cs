using System;

namespace DragoAnt.Shared.Mermaid.Flowchart.Interaction;

public class FlowchartInteractionCallback : FlowchartInteraction
{
    /// <inheritdoc />
    public FlowchartInteractionCallback(Func<MermaidPrintConfig, string> reference, string? toolTip = null)
        : base(reference, toolTip)
    {
    }
}