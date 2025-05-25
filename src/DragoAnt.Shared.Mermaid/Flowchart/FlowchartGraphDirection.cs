namespace DragoAnt.Shared.Mermaid.Flowchart;

public enum FlowchartGraphDirection
{
    /// <summary>
    /// None
    /// </summary>
    None = 0,

    /// <summary>
    /// top to bottom
    /// </summary>
    TB,

    /// <summary>
    /// top-down/ same as top to bottom
    /// </summary>
    TD,

    /// <summary>
    /// bottom to top
    /// </summary>
    BT,

    /// <summary>
    /// right to left
    /// </summary>
    RL,

    /// <summary>
    /// left to right
    /// </summary>
    LR
}

public enum FlowchartRelationLineStyle
{
    Line = 0,
    BoldLine = 1,
    Dots = 2
}

public enum FlowchartRelationLineEnding
{
    None = 0,
    Arrow = 1,
    Cross = 2,
    Circle = 3
}