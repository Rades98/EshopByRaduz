namespace Basket.Api.Events
{
    public sealed record BasketChangedEvent(Guid UserReference, Guid BasketReference);
}
