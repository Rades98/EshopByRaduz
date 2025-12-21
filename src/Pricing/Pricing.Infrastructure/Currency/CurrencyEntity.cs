namespace Pricing.Infrastructure.Currency
{
    public class CurrencyEntity
    {
        public required string Code { get; set; }

        public required string Symbol { get; set; }

        public decimal ExchangeRate { get; set; }

        public int Precision { get; set; }

        public bool IsMaster { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
