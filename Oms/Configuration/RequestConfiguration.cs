using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oms.Models;

namespace Oms.Configuration
{
    public class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.ToTable(nameof(Request), nameof(Oms));
            builder.Property(r => r.ProductName).HasMaxLength(256);
            builder.OwnsMany(p => p.Errors, t =>
            {
                t.ToTable(nameof(RequestError), nameof(Oms));
                t.Property(p => p.Message).HasMaxLength(512);
                t.HasKey("Id");
            });
        }
    }
}
