using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DragoAnt.Shared.Text;

public sealed class AdvStringBuilder
{
    private readonly StringBuilder _stringBuilder;
    private readonly string _identChunk;
    private string _ident;
    private bool _identInserted;

    public AdvStringBuilder(string identChunk = "  ")
        : this(new StringBuilder(), identChunk)
    {
    }

    public AdvStringBuilder(StringBuilder stringBuilder, string identChunk = "  ", bool identInserted = false)
    {
        _stringBuilder = stringBuilder ?? throw new ArgumentNullException(nameof(stringBuilder));
        _identChunk = identChunk ?? throw new ArgumentNullException(nameof(identChunk));
        _identInserted = identInserted;
        if (_identChunk.Length == 0)
        {
            throw new ArgumentException("Ident chunk can't has zero length");
        }
        _ident = string.Empty;
    }

    public void AddIdent(int ident = 1)
    {
        for (var i = 0; i < ident; i++)
        {
            _ident += _identChunk;
        }
    }

    public void RemoveIdent(int ident = 1)
    {
        for (var i = 0; i < ident; i++)
        {
            if (_ident.Length == 0)
            {
                return;
            }
#if NETSTANDARD
            _ident = _ident.Substring(0, _ident.Length - _identChunk.Length);
#else
                _ident = _ident[..^_identChunk.Length];
#endif
        }
    }

    public int Ident => _ident.Length / _identChunk.Length;

    public string GetIdent()
    {
        return _ident;
    }

    private void AppendIdent()
    {
        if (_identInserted)
        {
            return;
        }
        _stringBuilder.Append(GetIdent());
        _identInserted = true;
    }

    public AdvStringBuilder Append(bool value)
    {
        AppendIdent();
        _stringBuilder.Append(value);
        return this;
    }

    public AdvStringBuilder Append(byte value)
    {
        AppendIdent();
        _stringBuilder.Append(value);
        return this;
    }

    public AdvStringBuilder Append(char value)
    {
        AppendIdent();
        _stringBuilder.Append(value);
        return this;
    }

    public AdvStringBuilder Append(char value, int repeatCount)
    {
        AppendIdent();
        _stringBuilder.Append(value, repeatCount);
        return this;
    }

    public AdvStringBuilder Append(char[]? value)
    {
        AppendIdent();
        _stringBuilder.Append(value);
        return this;
    }

    public AdvStringBuilder Append(char[]? value, int startIndex, int charCount)
    {
        AppendIdent();
        _stringBuilder.Append(value, startIndex, charCount);
        return this;
    }

    public AdvStringBuilder Append(decimal value)
    {
        AppendIdent();
        _stringBuilder.Append(value);
        return this;
    }

    public AdvStringBuilder Append(double value)
    {
        AppendIdent();
        _stringBuilder.Append(value);
        return this;
    }

    public AdvStringBuilder Append(short value)
    {
        AppendIdent();
        _stringBuilder.Append(value);
        return this;
    }

    public AdvStringBuilder Append(int value)
    {
        AppendIdent();
        _stringBuilder.Append(value);
        return this;
    }

    public AdvStringBuilder Append(long value)
    {
        AppendIdent();
        _stringBuilder.Append(value);
        return this;
    }

    public AdvStringBuilder Append(object? value)
    {
        AppendIdent();
        _stringBuilder.Append(value);
        return this;
    }
        
#if !NETSTANDARD2_0
    public AdvStringBuilder Append(ReadOnlyMemory<char> value)
    {
        AppendIdent();
        _stringBuilder.Append(value);
        return this;
    }

    public AdvStringBuilder Append(ReadOnlySpan<char> value)
    {
        AppendIdent();
        _stringBuilder.Append(value);
        return this;
    }

    public AdvStringBuilder Insert(int index, ReadOnlySpan<char> value)
    {
        _stringBuilder.Insert(index, value);
        return this;
    }

    public AdvStringBuilder Append(StringBuilder? value, int startIndex, int count)
    {
        AppendIdent();
        _stringBuilder.Append(value, startIndex, count);
        return this;
    }

    public void CopyTo(int sourceIndex, Span<char> destination, int count)
    {
        _stringBuilder.CopyTo(sourceIndex, destination, count);
    }

    public bool Equals(ReadOnlySpan<char> span)
    {
        return _stringBuilder.Equals(span);
    }
        
    public AdvStringBuilder AppendJoin(char separator, params object?[] values)
    {
        AppendIdent();
        _stringBuilder.AppendJoin(separator, values);
        return this;
    }

    public AdvStringBuilder AppendJoin(char separator, params string?[] values)
    {
        AppendIdent();
        _stringBuilder.AppendJoin(separator, values);
        return this;
    }

    public AdvStringBuilder AppendJoin(string? separator, params object?[] values)
    {
        AppendIdent();
        _stringBuilder.AppendJoin(separator, values);
        return this;
    }

    public AdvStringBuilder AppendJoin(string? separator, params string?[] values)
    {
        AppendIdent();
        _stringBuilder.AppendJoin(separator, values);
        return this;
    }

    public AdvStringBuilder AppendJoin<T>(char separator, IEnumerable<T> values)
    {
        AppendIdent();
        _stringBuilder.AppendJoin(separator, values);
        return this;
    }

    public AdvStringBuilder AppendJoin<T>(string? separator, IEnumerable<T> values)
    {
        AppendIdent();
        _stringBuilder.AppendJoin(separator, values);
        return this;
    }
#else
        public AdvStringBuilder AppendJoin(char separator, params object?[] values)
        {
            AppendJoin(separator.ToString(), values);
            return this;
        }

        public AdvStringBuilder AppendJoin(char separator, params string?[] values)
        {
            AppendJoin(separator.ToString(), values);
            return this;
        }

        public AdvStringBuilder AppendJoin(string? separator, params object?[] values)
        {
            AppendIdent();
            _stringBuilder.Append(string.Join( separator, values));
            return this;
        }

        public AdvStringBuilder AppendJoin(string? separator, params string?[] values)
        {
            AppendIdent();
            _stringBuilder.Append(string.Join( separator, values));
            return this;
        }

        public AdvStringBuilder AppendJoin<T>(char separator, IEnumerable<T> values)
        {
            AppendJoin(separator, values.Cast<object>().ToArray());
            return this;
        }

        public AdvStringBuilder AppendJoin<T>(string? separator, IEnumerable<T> values)
        {
            AppendJoin(separator, values.Cast<object>().ToArray());
            return this;
        }
#endif
#if !NETSTANDARD
        public StringBuilder.ChunkEnumerator GetChunks()
        {
            return _stringBuilder.GetChunks();
        }
#endif


    public AdvStringBuilder Append(sbyte value)
    {
        AppendIdent();
        _stringBuilder.Append(value);
        return this;
    }

    public AdvStringBuilder Append(float value)
    {
        AppendIdent();
        _stringBuilder.Append(value);
        return this;
    }

    public AdvStringBuilder Append(string? value)
    {
        AppendIdent();
        _stringBuilder.Append(value);
        return this;
    }

    public AdvStringBuilder Append(string? value, int startIndex, int count)
    {
        AppendIdent();
        _stringBuilder.Append(value, startIndex, count);
        return this;
    }

    public AdvStringBuilder Append(StringBuilder? value)
    {
        AppendIdent();
        _stringBuilder.Append(value);
        return this;
    }


    public AdvStringBuilder Append(ushort value)
    {
        AppendIdent();
        _stringBuilder.Append(value);
        return this;
    }

    public AdvStringBuilder Append(uint value)
    {
        AppendIdent();
        _stringBuilder.Append(value);
        return this;
    }

    public AdvStringBuilder Append(ulong value)
    {
        AppendIdent();
        _stringBuilder.Append(value);
        return this;
    }

    public AdvStringBuilder AppendFormat(IFormatProvider? provider, string format, object? arg0)
    {
        AppendIdent();
        _stringBuilder.AppendFormat(provider, format, arg0);
        return this;
    }

    public AdvStringBuilder AppendFormat(IFormatProvider? provider, string format, object? arg0, object? arg1)
    {
        AppendIdent();
        _stringBuilder.AppendFormat(provider, format, arg0, arg1);
        return this;
    }

    public AdvStringBuilder AppendFormat(IFormatProvider? provider, string format, object? arg0, object? arg1, object? arg2)
    {
        AppendIdent();
        _stringBuilder.AppendFormat(provider, format, arg0, arg1, arg2);
        return this;
    }

    public AdvStringBuilder AppendFormat(IFormatProvider? provider, string format, params object?[] args)
    {
        AppendIdent();
        _stringBuilder.AppendFormat(provider, format, args);
        return this;
    }

    public AdvStringBuilder AppendFormat(string format, object? arg0)
    {
        AppendIdent();
        _stringBuilder.AppendFormat(format, arg0);
        return this;
    }

    public AdvStringBuilder AppendFormat(string format, object? arg0, object? arg1)
    {
        AppendIdent();
        _stringBuilder.AppendFormat(format, arg0, arg1);
        return this;
    }

    public AdvStringBuilder AppendFormat(string format, object? arg0, object? arg1, object? arg2)
    {
        AppendIdent();
        _stringBuilder.AppendFormat(format, arg0, arg1, arg2);
        return this;
    }

    public AdvStringBuilder AppendFormat(string format, params object?[] args)
    {
        AppendIdent();
        _stringBuilder.AppendFormat(format, args);
        return this;
    }

    public AdvStringBuilder AppendLine()
    {
        _stringBuilder.AppendLine();
        _identInserted = false;
        return this;
    }

    public AdvStringBuilder AppendLine(string? value)
    {
        AppendIdent();
        _stringBuilder.AppendLine(value);
        _identInserted = false;
        return this;
    }

    public AdvStringBuilder Clear()
    {
        _stringBuilder.Clear();
        _identInserted = false;
        return this;
    }

    public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
    {
        _stringBuilder.CopyTo(sourceIndex, destination, destinationIndex, count);
    }

    public int EnsureCapacity(int capacity)
    {
        return _stringBuilder.EnsureCapacity(capacity);
    }

    public bool Equals(StringBuilder? sb)
    {
        return _stringBuilder.Equals(sb);
    }

    public AdvStringBuilder Insert(int index, bool value)
    {
        _stringBuilder.Insert(index, value);
        return this;
    }

    public AdvStringBuilder Insert(int index, byte value)
    {
        _stringBuilder.Insert(index, value);
        return this;
    }

    public AdvStringBuilder Insert(int index, char value)
    {
        _stringBuilder.Insert(index, value);
        return this;
    }

    public AdvStringBuilder Insert(int index, char[]? value)
    {
        _stringBuilder.Insert(index, value);
        return this;
    }

    public AdvStringBuilder Insert(int index, char[]? value, int startIndex, int charCount)
    {
        _stringBuilder.Insert(index, value, startIndex, charCount);
        return this;
    }

    public AdvStringBuilder Insert(int index, decimal value)
    {
        _stringBuilder.Insert(index, value);
        return this;
    }

    public AdvStringBuilder Insert(int index, double value)
    {
        _stringBuilder.Insert(index, value);
        return this;
    }

    public AdvStringBuilder Insert(int index, short value)
    {
        _stringBuilder.Insert(index, value);
        return this;
    }

    public AdvStringBuilder Insert(int index, int value)
    {
        _stringBuilder.Insert(index, value);
        return this;
    }

    public AdvStringBuilder Insert(int index, long value)
    {
        _stringBuilder.Insert(index, value);
        return this;
    }

    public AdvStringBuilder Insert(int index, object? value)
    {
        _stringBuilder.Insert(index, value);
        return this;
    }

    public AdvStringBuilder Insert(int index, sbyte value)
    {
        _stringBuilder.Insert(index, value);
        return this;
    }

    public AdvStringBuilder Insert(int index, float value)
    {
        _stringBuilder.Insert(index, value);
        return this;
    }

    public AdvStringBuilder Insert(int index, string? value)
    {
        _stringBuilder.Insert(index, value);
        return this;
    }

    public AdvStringBuilder Insert(int index, string? value, int count)
    {
        _stringBuilder.Insert(index, value, count);
        return this;
    }

    public AdvStringBuilder Insert(int index, ushort value)
    {
        _stringBuilder.Insert(index, value);
        return this;
    }

    public AdvStringBuilder Insert(int index, uint value)
    {
        _stringBuilder.Insert(index, value);
        return this;
    }

    public AdvStringBuilder Insert(int index, ulong value)
    {
        _stringBuilder.Insert(index, value);
        return this;
    }

    public AdvStringBuilder Remove(int startIndex, int length)
    {
        _stringBuilder.Remove(startIndex, length);
        return this;
    }

    public AdvStringBuilder Replace(char oldChar, char newChar)
    {
        _stringBuilder.Replace(oldChar, newChar);
        return this;
    }

    public AdvStringBuilder Replace(char oldChar, char newChar, int startIndex, int count)
    {
        _stringBuilder.Replace(oldChar, newChar, startIndex, count);
        return this;
    }

    public AdvStringBuilder Replace(string oldValue, string? newValue)
    {
#if NETSTANDARD
        newValue ??= string.Empty; 
#endif 
        _stringBuilder.Replace(oldValue, newValue);
        return this;
    }

    public AdvStringBuilder Replace(string oldValue, string? newValue, int startIndex, int count)
    {
#if NETSTANDARD
        newValue ??= string.Empty; 
#endif
        _stringBuilder.Replace(oldValue, newValue, startIndex, count);
        return this;
    }

    public string ToString(int startIndex, int length)
    {
        return _stringBuilder.ToString(startIndex, length);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return _stringBuilder.ToString();
    }

    public int Capacity
    {
        get => _stringBuilder.Capacity;
        set => _stringBuilder.Capacity = value;
    }

    public char this[int index]
    {
        get => _stringBuilder[index];
        set => _stringBuilder[index] = value;
    }

    public int Length
    {
        get => _stringBuilder.Length;
        set => _stringBuilder.Length = value;
    }

    public int MaxCapacity => _stringBuilder.MaxCapacity;
}