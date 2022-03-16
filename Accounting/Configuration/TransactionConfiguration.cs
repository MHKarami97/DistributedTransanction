using Accounting.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Configuration
{
    public class TransactionConfiguration : IEntityTypeConfiguration<TransactionalProcess>
    {
        public void Configure(EntityTypeBuilder<TransactionalProcess> builder)
        {
            builder.ToTable(nameof(TransactionalProcess), "Cas");
        }
    }
}
