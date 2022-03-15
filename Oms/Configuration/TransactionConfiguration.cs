using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oms.Models;

namespace Oms.Configuration
{
    public class TransactionConfiguration : IEntityTypeConfiguration<TransactionalProcess>
    {
        public void Configure(EntityTypeBuilder<TransactionalProcess> builder)
        {
            builder.ToTable(nameof(TransactionalProcess), nameof(Oms));
        }
    }
}
