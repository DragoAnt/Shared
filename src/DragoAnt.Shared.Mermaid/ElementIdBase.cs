using System;

namespace DragoAnt.Shared.Mermaid;

public abstract class ElementIdBase : ElementBase, IEquatable<ElementIdBase>
{
    public static implicit operator string(ElementIdBase v) => v.Id;

    /// <inheritdoc />
    protected ElementIdBase(string id)
    {
        Id = id;
    }

    public string Id { get; private set; }

    public void ChangeId(string newId)
    {
        Id = newId;
    }

    /// <inheritdoc />
    public bool Equals(ElementIdBase? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        return Id == other.Id;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }
        if (ReferenceEquals(this, obj))
        {
            return true;
        }
        return obj.GetType() == GetType() && Equals((ElementIdBase)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return Id.GetHashCode();
    }
}