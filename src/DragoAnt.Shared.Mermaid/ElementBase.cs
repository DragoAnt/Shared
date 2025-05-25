using DragoAnt.Shared.Text;

namespace DragoAnt.Shared.Mermaid;

public abstract class ElementBase
{
    protected internal abstract bool Print(AdvStringBuilder builder, MermaidPrintConfig config);

    /// <inheritdoc />
    public override string? ToString()
    {
        return ToString(MermaidPrintConfig.Normal);
    }
        
    public string? ToString(MermaidPrintConfig config)
    {
        var builder = new AdvStringBuilder();
        Print(builder, config);
        return builder.ToString();
    }
}