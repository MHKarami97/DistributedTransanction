using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saga.V2;

namespace Oms.Configuration
{
    public class DistributedTransactionModelConfiguration : IEntityTypeConfiguration<DistributedTransactionModel>
    {
        public void Configure(EntityTypeBuilder<DistributedTransactionModel> builder)
        {
            builder.ToTable(nameof(DistributedTransactionModel), nameof(Oms));
            builder.Property(c => c.CollaborationId).HasComputedColumnSql("[Id]");
        }
    }
}
