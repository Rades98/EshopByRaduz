using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stock.Infrastructure.StockItems.StockUnits.CostSnapshot
{
    internal class CostSnapshotEntityConfiguration : IEntityTypeConfiguration<CostSnapshotEntity>
    {
        public void Configure(EntityTypeBuilder<CostSnapshotEntity> builder)
        {

        }
    }
}
