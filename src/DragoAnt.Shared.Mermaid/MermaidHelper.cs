using System;
using System.Collections.Generic;
using System.Linq;

namespace DragoAnt.Shared.Mermaid;

public static class MermaidHelper
{
    public static string ReplaceRestrictedSymbols(string value, char replace = '_')
    {
        return TransformString(value, c => ReplaceRestrictedSymbols(c, replace));
    }

    public static string EscapeString(string value, MermaidPrintConfig config)
    {
        Func<char, string?> escape = config switch
        {
            MermaidPrintConfig.Normal => EscapeNormal,
            MermaidPrintConfig.ForHtml => EscapeForHtml,
            _ => throw new ArgumentOutOfRangeException(nameof(config))
        };
        value = SaveMarkers(value);

        var ret = new string(TransformString(value, escape).ToArray());

        ret = RestoreMarkers(ret);
        return ret;
    }

    private static string TransformString(string value, Func<char, char> transform)
    {
        return new string(value.Select(transform).ToArray());
    }

    private static IEnumerable<char> TransformString(string value, Func<char, string?> escape)
    {
        foreach (var symbol in value)
        {
            var escaped = escape(symbol);
            if (escaped is null)
            {
                yield return symbol;
            }
            else
            {
                foreach (var s in escaped)
                {
                    yield return s;
                }
            }
        }
    }

    private static string SaveMarkers(string value)
    {
        value = MarkerReplace(value, MarkerStartB, ReplacementMarkerStartB);
        value = MarkerReplace(value, MarkerEndB, ReplacementMarkerEndB);

        value = MarkerReplace(value, MarkerStartI, ReplacementMarkerStartI);
        value = MarkerReplace(value, MarkerEndI, ReplacementMarkerEndI);
        return value;
    }

    private static string RestoreMarkers(string value)
    {
        value = MarkerReplace(value, ReplacementMarkerStartB, MarkerStartB);
        value = MarkerReplace(value, ReplacementMarkerEndB, MarkerEndB);

        value = MarkerReplace(value, ReplacementMarkerStartI, MarkerStartI);
        value = MarkerReplace(value, ReplacementMarkerEndI, MarkerEndI);
        return value;
    }

    private const string MarkerStartB = "<b>";
    private const string ReplacementMarkerStartB = "####start_b####";
    private const string MarkerEndB = "</b>";
    private const string ReplacementMarkerEndB = "####end_b####";

    private const string MarkerStartI = "<i>";
    private const string ReplacementMarkerStartI = "####start_i####";
    private const string MarkerEndI = "</i>";
    private const string ReplacementMarkerEndI = "####end_i####";

    private static string MarkerReplace(string value, string marker, string replacement)
    {
        return value.Replace(marker, replacement);
    }

    private static char ReplaceRestrictedSymbols(char symbol, char replace)
    {
        switch (symbol)
        {
            case '"':
            case '&':
            case '%':
            case '\'':
            case '(':
            case ')':
            case ';':
            case '<':
            case '=':
            case '>':
            case '[':
            case ']':
            case '{':
            case '|':
            case '}':
            case '~':
                return replace;
            default:
                return symbol;
        }
    }

    private static string? EscapeNormal(char symbol)
    {
        return symbol switch
        {
            '"' => "&quot",
            '&' => "&amp",
            '%' => "&#37",
            '\'' => "&#39",
            '(' => "&#40",
            ')' => "&#41",
            ';' => "&#59",
            '<' => "&lt",
            '=' => "&#61",
            '>' => "&gt",
            '[' => "&#91",
            ']' => "&#93",
            '{' => "&#123",
            '|' => "&#124",
            '}' => "&#125",
            '~' => "&#126",
            _ => null
        };
    }

    private static string? EscapeForHtml(char symbol)
    {
        return symbol switch
        {
            '<' => "&amplt",
            '>' => "&ampgt",
            '"' => "&ampquot",
            '%' => "&amp#37",
            '(' => "&amp#40",
            ')' => "&amp#41",
            '{' => "&amp#123",
            '}' => "&amp#125",
            '[' => "&amp#91",
            ']' => "&amp#93",
            '~' => "&amp#126",
            _ => EscapeNormal(symbol)
        };
    }
}