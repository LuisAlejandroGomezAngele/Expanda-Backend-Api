using ApiExpanda.Domain.Modules.Catalogos.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiExpanda.Infrastructure.Modules.Catalogos.Data.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");
        
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(255);
            
        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(255);
            
        builder.Property(c => c.Rfc)
            .HasMaxLength(255);
            
        builder.Property(c => c.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
            
        builder.Property(c => c.CreateAt)
            .IsRequired();
            
        // Índice único en el código
        builder.HasIndex(c => c.Code)
            .IsUnique()
            .HasDatabaseName("IX_Companies_Code");
            
        // Índice en IsActive para filtrado
        builder.HasIndex(c => c.IsActive)
            .HasDatabaseName("IX_Companies_IsActive");
    }
}
