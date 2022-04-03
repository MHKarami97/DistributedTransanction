using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saga.V2;

namespace Accounting.Configuration
{
    public class DistributedTransactionModelConfiguration : IEntityTypeConfiguration<DistributedTransactionModel>
    {
        public void Configure(EntityTypeBuilder<DistributedTransactionModel> builder)
        {
            builder.ToTable(nameof(DistributedTransactionModel), nameof(Accounting));
        }
    }
}
