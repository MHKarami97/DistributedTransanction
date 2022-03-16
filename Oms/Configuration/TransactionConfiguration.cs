using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oms.Models;

namespace Oms.Configuration
{
    public class TransactionConfiguration : IEntityTypeConfiguration<RequestContext>
    {
        public void Configure(EntityTypeBuilder<RequestContext> builder)
        {
            builder.ToTable(nameof(RequestContext), nameof(Oms));
        }
    }
}
