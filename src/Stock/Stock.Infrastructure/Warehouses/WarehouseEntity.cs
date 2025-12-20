using Stock.Domain.Warehouses;

namespace Stock.Infrastructure.Warehouses
{
    public class WarehouseEntity
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Street { get; set; }

        public required string City { get; set; }

        public required string PostalCode { get; set; }

        public required string Country { get; set; }

        public WarehouseType Type { get; set; }
    }
}
