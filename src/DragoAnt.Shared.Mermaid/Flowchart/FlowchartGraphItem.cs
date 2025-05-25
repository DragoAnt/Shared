using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DragoAnt.Shared.Mermaid.Flowchart.Interaction;
using DragoAnt.Shared.Text;

namespace DragoAnt.Shared.Mermaid.Flowchart;

[DebuggerDisplay("{Id}, Direction:{Direction}, Shape: {Shape}, Children:{Children.Count}")]
public sealed class FlowchartGraphItem : ElementIdBase
{
    internal readonly List<FlowchartGraphItem> _children;
    internal string? _styleClassId;

    internal FlowchartGraphItem(string id, FlowchartGraph graph, FlowchartGraphItem? parent, string? caption = null, int countChildren = 0)
        :base(id)
    {
        Graph = graph;
        Parent = parent;
        Parent = parent;
        Caption = caption;
        _children = new List<FlowchartGraphItem>(countChildren);
    }

    public string? Caption { get; set; }
    public FlowchartGraph Graph { get; }
    public FlowchartGraphItem? Parent { get; }
    public IReadOnlyCollection<FlowchartGraphItem> Children => _children;

    public FlowchartGraphDirection Direction { get; set; } = FlowchartGraphDirection.None;
    public FlowchartShape Shape { get; set; } = FlowchartShape.Box;

    public string? StyleClassId
    {
        get => _styleClassId;
        set { Graph.SetStyleClassToItem(value, this); }
    }

    public FlowchartGraphItem Add(string id, string? name = null, int countChildren = 0)
    {
        var item = new FlowchartGraphItem(id, Graph, this, name, countChildren);
        Graph.AddItem(item);
        return item;
    }

    public FlowchartInteraction? Interaction { get; internal set; }

    /// <inheritdoc />
    protected internal override bool Print(AdvStringBuilder builder, MermaidPrintConfig config)
    {
        void PrintNode(FlowchartShape shape)
        {
            builder.Append(MermaidHelper.EscapeString(Id, config));
            if (string.IsNullOrWhiteSpace(Caption) && shape == FlowchartShape.Box)
            {
                return;
            }
            var caption = MermaidHelper.EscapeString(Caption ?? Id, config);
            PrintCaption(builder, caption, shape);
        }

        bool AddDirectionValue(string? prefix)
        {
            if (Direction == FlowchartGraphDirection.None)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(prefix))
            {
                builder.Append(prefix);
            }
            builder.Append(' ');
            builder.Append(Direction.ToString());
            return true;
        }

        if (Parent == null)
        {
            builder.Append("flowchart");
            AddDirectionValue(null);
            builder.AppendLine();
            builder.AddIdent();

            AppendChildren(builder, config);
        }
        else
        {
            if (Children.Count == 0)
            {
                PrintNode(Shape);
            }
            else
            {
                builder.Append("subgraph");
                builder.Append(' ');
                PrintNode(FlowchartShape.Box);
                builder.AppendLine();
                builder.AddIdent();
                if(AddDirectionValue("direction"))
                {
                    builder.AppendLine();
                }

                AppendChildren(builder,config);

                builder.RemoveIdent();
                builder.Append("end");
            }
        }
        return true;
    }

    internal static void PrintCaption(AdvStringBuilder builder, string caption, FlowchartShape shape)
    {
        void PrintShape(string start, string end)
        {
            builder.Append(start);
            builder.Append(caption);
            builder.Append(end);
        }

        switch (shape)
        {
            case FlowchartShape.Box:
                PrintShape("[", "]");
                break;
            case FlowchartShape.BoxRoundEdges:
                PrintShape("(", ")");
                break;
            case FlowchartShape.Stadium:
                PrintShape("([", "])");
                break;
            case FlowchartShape.Subroutine:
                PrintShape("[[", "]]");
                break;
            case FlowchartShape.Cylindrical:
                PrintShape("[(", ")]");
                break;
            case FlowchartShape.Circle:
                PrintShape("((", "))");
                break;
            case FlowchartShape.Asymmetric:
                PrintShape(">", "]");
                break;
            case FlowchartShape.Rhombus:
                PrintShape("{", "}");
                break;
            case FlowchartShape.Hexagon:
                PrintShape("{{", "}}");
                break;
            case FlowchartShape.Parallelogram:
                PrintShape("[/", "/]");
                break;
            case FlowchartShape.ParallelogramAlt:
                PrintShape(@"[\", @"\]");
                break;
            case FlowchartShape.Trapezoid:
                PrintShape("[/", @"\]");
                break;
            case FlowchartShape.TrapezoidAlt:
                PrintShape(@"[\", "/]");
                break;
            case FlowchartShape.DoubleCircle:
                PrintShape("(((", ")))");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(shape), shape, null);
        }
    }

    private void AppendChildren(AdvStringBuilder builder, MermaidPrintConfig config)
    {
        foreach (var child in _children.OrderBy(c => c.Id))
        {
            if (child.Print(builder, config))
            {
                builder.AppendLine();
            }
        }
    }
}