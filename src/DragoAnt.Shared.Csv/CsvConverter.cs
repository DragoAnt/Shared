using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using CsvHelper;
using CsvHelper.Configuration;
using DragoAnt.Shared.Resources;

namespace DragoAnt.Shared.Csv;

public static class CsvConverter
{
    public static IEnumerable<TOutput> ReadCsvFromResFile<TData, TOutput>(string relativePath, Func<TData, TOutput> convert, Assembly? assembly = null)
        where TData : new()
    {
        assembly ??= Assembly.GetCallingAssembly();
        var file = ResFile.Relative(relativePath, assembly);
        return ReadCsv(file, convert);
    }

    public static IEnumerable<TOutput> ReadCsv<TData, TOutput>(ResFile file, Func<TData, TOutput> convert)
        where TData : new()
    {
        using var stream = file.ReadStream();
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            IncludePrivateMembers = true,
            DetectDelimiter = true,
            HasHeaderRecord = true,
            HeaderValidated = null,
            MissingFieldFound = null
        });

        var data = new TData();
        var records = csv.EnumerateRecords(data);
        foreach (var record in records)
        {
            yield return convert(record);
        }
    }

    /// <summary>
    /// Returns value if it doesn't equal null or empty otherwise returns null
    /// </summary>
    /// <param name="value"></param>
    /// <param name="convert"></param>
    /// <param name="deflt"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? NullOrEmpty<T>(string? value, Func<string, T> convert, T? deflt)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        var r = string.IsNullOrEmpty(value) || value is null ? deflt : convert(value);
        return r;
    }

    /// <summary>
    /// Returns value if it doesn't equal null or whitespace otherwise returns null
    /// </summary>
    /// <param name="value"></param>
    /// <param name="convert"></param>
    /// <param name="deflt"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? NullOrWhitespace<T>(string? value, Func<string, T> convert, T? deflt)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        var r = string.IsNullOrWhiteSpace(value) || value is null ? deflt : convert(value);
        return r;
    }

    /// <summary>
    /// Returns value if it doesn't equal null or empty otherwise returns null
    /// </summary>
    /// <param name="value"></param>
    /// <param name="deflt"></param>
    /// <returns></returns>
    public static string? NullOrEmpty(string? value, string? deflt = null)
    {
        return string.IsNullOrEmpty(value) ? deflt : value;
    }

    /// <summary>
    /// Returns value if it doesn't equal null or whitespace otherwise returns null
    /// </summary>
    /// <param name="value"></param>
    /// <param name="deflt"></param>
    /// <returns></returns>
    public static string? NullOrWhitespace(string? value, string? deflt = null)
    {
        return string.IsNullOrWhiteSpace(value) ? deflt : value;
    }

    /// <summary>
    /// Returns value if it doesn't equal null or empty otherwise throws <see cref="ArgumentException"/>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool Bool(string? value)
    {
        return BoolNullable(value) ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Returns value if it doesn't equal null or empty otherwise returns null
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool? BoolNullable(string? value)
    {
        return NullOrEmpty<bool?>(value, v => bool.Parse(v), null);
    }

    /// <summary>
    /// Returns value if it doesn't equal null or empty otherwise throws <see cref="ArgumentException"/>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="provider"></param>
    /// <param name="styles"></param>
    /// <returns></returns>
    public static DateTime DateTime(string? value, IFormatProvider? provider = null, DateTimeStyles styles = DateTimeStyles.None)
    {
        return DateTimeNullable(value, provider, styles) ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Returns value if it doesn't equal null or empty otherwise returns null
    /// </summary>
    /// <param name="value"></param>
    /// <param name="provider"></param>
    /// <param name="styles"></param>
    /// <returns></returns>
    public static DateTime? DateTimeNullable(string? value, IFormatProvider? provider = null, DateTimeStyles styles = DateTimeStyles.None)
    {
        provider ??= DateTimeFormatInfo.InvariantInfo;
        return NullOrEmpty<DateTime?>(value, v => System.DateTime.Parse(v, provider, styles), null);
    }

    /// <summary>
    /// Returns value if it doesn't equal null or empty otherwise throws <see cref="ArgumentException"/>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="style"></param>
    /// <returns></returns>
    public static decimal Decimal(string? value, NumberStyles style = NumberStyles.Any)
    {
        return DecimalNullable(value, style) ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Returns value if it doesn't equal null or empty otherwise returns null
    /// </summary>
    /// <param name="value"></param>
    /// <param name="style"></param>
    /// <returns></returns>
    public static decimal? DecimalNullable(string? value, NumberStyles style = NumberStyles.Any)
    {
        return NullOrEmpty<decimal?>(value, v => decimal.Parse(v, style, NumberFormatInfo.InvariantInfo), null);
    }

    /// <summary>
    /// Returns value if it doesn't equal null or empty otherwise throws <see cref="ArgumentException"/>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T Enum<T>(string? value)
        where T : struct, Enum
    {
        return EnumNullable<T>(value) ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Returns value if it doesn't equal null or empty otherwise returns null
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T? EnumNullable<T>(string? value)
        where T : struct, Enum
    {
        // ReSharper disable once ConvertClosureToMethodGroup
#if !NETSTANDARD2_0
        return NullOrEmpty<T?>(value, v => System.Enum.Parse<T>(v), null);
#else
            return NullOrEmpty<T?>(value, v => (T)System.Enum.Parse(typeof(T), v), null);
#endif
    }
}