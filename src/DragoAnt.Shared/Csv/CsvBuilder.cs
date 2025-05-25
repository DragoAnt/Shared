using System.Linq;
using System.Text;

namespace DragoAnt.Shared.Csv;

public sealed class CsvBuilder
{
    private char Delimiter { get; }

    private readonly char[] _escapingChars;

    private readonly StringBuilder _sbuilder = new();

    public CsvBuilder(char delimiter = ',')
    {
        Delimiter = delimiter;
        _escapingChars = [Delimiter, '"', '\r', '\n', '\t', ' '];
    }

    private string GetCsvValue(string? value)
    {
        if (value == null)
        {
            return string.Empty;
        }
        if (_escapingChars.Intersect(value).Any())
        {
            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }
        return value;
    }

    public void AddRow(string?[] values)
    {
#if NETSTANDARD2_0
            _sbuilder.AppendLine(string.Join(Delimiter.ToString(), values.Select(GetCsvValue).ToArray()));
#else
        _sbuilder.AppendLine(string.Join(Delimiter, values.Select(GetCsvValue)));
#endif
    }

    public string Build()
    {
        return _sbuilder.ToString();
    }
}