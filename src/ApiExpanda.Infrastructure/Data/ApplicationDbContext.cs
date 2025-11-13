using Microsoft.EntityFrameworkCore;
using ApiExpanda.Domain.Entities;
using ApiExpanda.Infrastructure.Modules.Catalogos.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CategoryEntity = ApiExpanda.Domain.Modules.Catalogos.Entities.Category;
using ProductEntity = ApiExpanda.Domain.Modules.Catalogos.Entities.Product;
using CompanyEntity = ApiExpanda.Domain.Modules.Catalogos.Entities.Company;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configuraciones de entidades modulares - Catalogos
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
    }

    // Legacy (mantener temporalmente para migración)
    public new DbSet<User> Users { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    
    // Módulo Catalogos - Entidades Modulares
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<CompanyEntity> Companies { get; set; }
}
