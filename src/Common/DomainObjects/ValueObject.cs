namespace DomainObjects;

public abstract class ValueObject<TSelf, TValue>(TValue value)
    where TSelf : ValueObject<TSelf, TValue>
{
    public TValue Value { get; private set; } = value;

    public static bool operator !=(ValueObject<TSelf, TValue>? a, ValueObject<TSelf, TValue>? b) => !(a == b);

    public static bool operator ==(ValueObject<TSelf, TValue>? a, ValueObject<TSelf, TValue>? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject<TSelf, TValue>)obj;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(1, (current, obj) =>
            {
                unchecked
                {
                    return HashCode.Combine(current, obj);
                }
            });
    }

    protected virtual IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
