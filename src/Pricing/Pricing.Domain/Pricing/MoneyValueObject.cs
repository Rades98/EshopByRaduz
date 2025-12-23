using DomainObjects;

namespace Pricing.Domain.Pricing
{
    public sealed class MoneyValueObject :
        ValueObject<MoneyValueObject, (decimal Amount, string CurrencyCode)>,
        IValueObject<MoneyValueObject, decimal, string>
    {
        public decimal Amount { get; private set; }

        public string CurrencyCode { get; private set; }

        private MoneyValueObject(decimal amount, string currencyCode) : base(new(amount, currencyCode))
        {
            Amount = amount;
            CurrencyCode = currencyCode;
        }

        public MoneyValueObject Multiply(int quantity) => new(Amount * quantity, CurrencyCode);

        public static Result<MoneyValueObject> Create(decimal Amount, string CurrencyCode)
        {
            if (Amount < 0)
            {
                return Result<MoneyValueObject>.Failure("MONEY_ERROR_AMOUNT_MUST_BE_POSITIVE");
            }
            if (string.IsNullOrWhiteSpace(CurrencyCode))
            {
                return Result<MoneyValueObject>.Failure("MONEY_ERROR_CURRENCY_NULL_OR_EMPTY");
            }

            return Result<MoneyValueObject>.Success(new MoneyValueObject(Amount, CurrencyCode));
        }
    }
}
