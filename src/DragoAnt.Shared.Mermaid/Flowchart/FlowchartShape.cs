namespace DragoAnt.Shared.Mermaid.Flowchart;

public enum FlowchartShape
{
    /// <summary>
    /// [This is the text in the box]
    /// </summary>
    Box=0,
    /// <summary>
    /// (This is the text in the box)
    /// </summary>
    BoxRoundEdges,
    /// <summary>
    /// ([This is the text in the box])
    /// </summary>
    Stadium,
    /// <summary>
    /// [[This is the text in the box]]
    /// </summary>
    Subroutine,
    /// <summary>
    /// [(Database)]
    /// </summary>
    Cylindrical,
    /// <summary>
    /// ((This is the text in the circle))
    /// </summary>
    Circle,
    /// <summary>
    /// &gt;This is the text in the box]
    /// </summary>
    Asymmetric,
    /// <summary>
    /// {This is the text in the box}
    /// </summary>
    Rhombus,
    /// <summary>
    /// {{This is the text in the box}}
    /// </summary>
    Hexagon,
    /// <summary>
    /// [/This is the text in the box/]
    /// </summary>
    Parallelogram,
    /// <summary>
    /// [\This is the text in the box\]
    /// </summary>
    ParallelogramAlt,
    /// <summary>
    /// [/Christmas\]
    /// </summary>
    Trapezoid,
    /// <summary>
    /// [\Christmas/]
    /// </summary>
    TrapezoidAlt,
    /// <summary>
    /// (((This is the text in the circle)))
    /// </summary>
    DoubleCircle
}