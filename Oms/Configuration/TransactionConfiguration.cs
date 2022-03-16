using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oms.Repository;

namespace Oms.Configuration
{
    public class DistributedTransactionModelConfiguration : IEntityTypeConfiguration<DistributedTransactionModel>
    {
        public void Configure(EntityTypeBuilder<DistributedTransactionModel> builder)
        {
            builder.ToTable(nameof(DistributedTransactionModel), nameof(Oms));
        }
    }
}
