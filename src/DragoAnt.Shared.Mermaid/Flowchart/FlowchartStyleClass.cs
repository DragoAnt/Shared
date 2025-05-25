using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DragoAnt.Shared.Text;

namespace DragoAnt.Shared.Mermaid.Flowchart;

[DebuggerDisplay("{Id}")]
public sealed class FlowchartStyleClass : ElementIdBase
{
    internal FlowchartStyleClass(string id, FlowchartGraph graph)
        : base(id)
    {
        Graph = graph;
    }

    internal FlowchartGraph Graph { get; }

    internal Dictionary<string, string> Modifiers { get; private set; } = new();

    internal List<FlowchartGraphItem> Items { get; } = [];

    public FlowchartStyleClass Copy(string id)
    {
        return Copy(id, Graph);
    }

    internal FlowchartStyleClass Copy(string id, FlowchartGraph graph)
    {
        return new FlowchartStyleClass(id, graph) { Modifiers = Modifiers.ToDictionary(pair => pair.Key, pair => pair.Value) };
    }

    public FlowchartStyleClass SetModifier(string name, string? value)
    {
        if (name == null)
        {
            throw new ArgumentNullException(nameof(name));
        }
        if (string.IsNullOrEmpty(value))
        {
            Modifiers.Remove(name);
        }
        else
        {
            Modifiers[name] = value!;
        }
        return this;
    }

    protected internal override bool Print(AdvStringBuilder builder, MermaidPrintConfig config)
    {
        if (Items.Count == 0 || Modifiers.Count == 0)
        {
            return false;
        }

        builder.Append("classDef ");
        builder.Append(Id);
        builder.Append(' ');
        builder.AppendJoin(',', Modifiers.Select(m => $"{m.Key}:{m.Value}"));
        builder.AppendLine();

        builder.Append("class ");
        builder.AppendJoin(',', Items.Select(i => i.Id));
        builder.Append(' ');
        builder.Append(Id);

        return true;
    }
}