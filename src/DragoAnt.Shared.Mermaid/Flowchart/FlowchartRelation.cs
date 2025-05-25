using System;
using DragoAnt.Shared.Text;
using static DragoAnt.Shared.Mermaid.Flowchart.FlowchartRelationLineEnding;
using static DragoAnt.Shared.Mermaid.Flowchart.FlowchartRelationLineStyle;

namespace DragoAnt.Shared.Mermaid.Flowchart;

public sealed class FlowchartRelation:ElementBase
{
    private int _lineLength;

    public FlowchartRelation(
        FlowchartGraphItem leftItem,
        FlowchartGraphItem rightItem,
        string? caption = null,
        FlowchartRelationLineEnding leftItemEnding = None,
        FlowchartRelationLineStyle lineStyle = Line,
        int lineLength = 0,
        FlowchartRelationLineEnding rightItemEnding = Arrow)
    {
        LeftItem = leftItem;
        RightItem = rightItem;
        Caption = caption;
        LeftItemEnding = leftItemEnding;
        LineStyle = lineStyle;
        LineLength = lineLength;
        RightItemEnding = rightItemEnding;
    }

    public FlowchartGraphItem LeftItem { get; set; }
    public FlowchartGraphItem RightItem { get; set; }
    public string? Caption { get; set; }
    public FlowchartRelationLineEnding LeftItemEnding { get; set; }
    public FlowchartRelationLineStyle LineStyle { get; set; }

    public int LineLength
    {
        get => _lineLength;
        set
        {
            if (_lineLength < 0)
            {
                throw new ArgumentException("Can't be less than 0", nameof(value));
            }
            _lineLength = value;
        }
    }

    public FlowchartRelationLineEnding RightItemEnding { get; set; }

    /// <inheritdoc />
    protected internal override bool Print(AdvStringBuilder builder, MermaidPrintConfig config)
    {
        var lineLength = _lineLength;
        if (RightItemEnding == None || LineStyle == Dots)
        {
            lineLength += 1;
        }

        void AddLineLength(char lineChar)
        {
            switch (lineLength)
            {
                case 0:
                    return;
                case 1:
                    builder.Append(lineChar);
                    return;
                default:
                    builder.Append(new string(lineChar, _lineLength));
                    break;
            }
        }

        builder.Append(MermaidHelper.EscapeString(LeftItem.Id, config));
        builder.Append(' ');
        switch(LeftItemEnding)
        {
            case None:
                break;
            case Arrow:
                builder.Append('<');
                break;
            case Cross:
                builder.Append('x');
                break;
            case Circle:
                builder.Append('o');
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        switch (LineStyle)
        {
            case Line:
                builder.Append('-');
                AddLineLength('-');
                builder.Append('-');
                break;
            case BoldLine:
                builder.Append('=');
                AddLineLength('=');
                builder.Append('=');
                break;
            case Dots:
                builder.Append('-');
                AddLineLength('.');
                builder.Append('-');
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
            
        switch(RightItemEnding)
        {
            case None:
                break;
            case Arrow:
                builder.Append('>');
                break;
            case Cross:
                builder.Append('x');
                break;
            case Circle:
                builder.Append('o');
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
            
        builder.Append(' ');
            
        if (!string.IsNullOrWhiteSpace(Caption))
        {
            builder.Append('|');
            builder.Append(MermaidHelper.EscapeString(Caption!, config));
            builder.Append('|');
        }
        builder.Append(MermaidHelper.EscapeString(RightItem.Id, config));

        return true;
    }
}