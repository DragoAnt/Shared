namespace DragoAnt.Shared.Mermaid.Flowchart.Interaction;

public class FlowchartInteractionTooltip : FlowchartInteractionCallback
{
    /// <inheritdoc />
    public FlowchartInteractionTooltip(string toolTip) 
        : base(_ => $"call foo{Guid.NewGuid():N}()",toolTip)
    {
    }
}