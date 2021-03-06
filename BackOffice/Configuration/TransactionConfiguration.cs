using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saga.V2;

namespace BackOffice.Configuration
{
    public class DistributedTransactionModelConfiguration : IEntityTypeConfiguration<DistributedTransactionModel>
    {
        public void Configure(EntityTypeBuilder<DistributedTransactionModel> builder)
        {
            builder.ToTable(nameof(DistributedTransactionModel), nameof(BackOffice));
        }
    }
}
