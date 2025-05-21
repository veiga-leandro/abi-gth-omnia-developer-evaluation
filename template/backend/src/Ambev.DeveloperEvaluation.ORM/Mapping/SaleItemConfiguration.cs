using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.ToTable("SaleItem");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.SaleId).IsRequired();
            builder.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
            builder.Property(x => x.ProductName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.Discount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.IsCancelled).IsRequired();

            builder.HasOne<Sale>()
                   .WithMany(s => s.Items)
                   .HasForeignKey(i => i.SaleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
