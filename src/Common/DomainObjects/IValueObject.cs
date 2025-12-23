namespace DomainObjects
{
    public interface IValueObject<TSelf, TValue>
        where TSelf : IValueObject<TSelf, TValue>
    {
        static abstract Result<TSelf> Create(TValue value);
    }

    public interface IValueObject<TSelf, TValue, TValue2>
        where TSelf : IValueObject<TSelf, TValue, TValue2>
    {
        static abstract Result<TSelf> Create(TValue value, TValue2 value2);
    }
}
