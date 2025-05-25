using System;

namespace DragoAnt.Shared.Tables;

public class TableWriterColumn
{
    public TableWriterColumn(string name, Type columnType)
    {
        Name = name;
        ColumnType = columnType;
    }

    public string Name { get; }
    public Type ColumnType { get; }

    public void Deconstruct(out string name, out Type columnType)
    {
        name = Name;
        columnType = ColumnType;
    }
}