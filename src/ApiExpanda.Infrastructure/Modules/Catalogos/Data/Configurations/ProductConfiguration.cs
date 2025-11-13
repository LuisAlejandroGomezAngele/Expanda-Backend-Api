using ApiExpanda.Domain.Modules.Catalogos.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiExpanda.Infrastructure.Modules.Catalogos.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.ProductId);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.ImgUrl)
            .HasMaxLength(500);

        builder.Property(p => p.ImgUrlLocal)
            .HasMaxLength(500);

        builder.Property(p => p.SKU)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Stock)
            .IsRequired();

        builder.Property(p => p.CreationDate)
            .IsRequired();

        builder.Property(p => p.UpdateDate);

        // Índice único en SKU para evitar duplicados
        builder.HasIndex(p => p.SKU)
            .IsUnique();

        // Índice en CategoryId para mejorar performance de queries
        builder.HasIndex(p => p.CategoryId);

        // Relación con Category
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
