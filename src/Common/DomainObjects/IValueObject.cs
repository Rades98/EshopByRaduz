namespace DomainObjects
{
    public interface IValueObject<TSelf, TValue>
    where TSelf : IValueObject<TSelf, TValue>
    {
        static abstract Result<TSelf> Create(TValue value);
    }
}
