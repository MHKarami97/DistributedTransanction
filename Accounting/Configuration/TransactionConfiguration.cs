using Accounting.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cas.Configuration
{
    public class TransactionConfiguration : IEntityTypeConfiguration<TransactionalProcess>
    {
        public void Configure(EntityTypeBuilder<TransactionalProcess> builder)
        {
            builder.ToTable(nameof(TransactionalProcess), nameof(Cas));
        }
    }
}
