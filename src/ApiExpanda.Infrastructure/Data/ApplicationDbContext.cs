using Microsoft.EntityFrameworkCore;
using ApiExpanda.Domain.Entities;
using ApiExpanda.Domain.Modules.Catalogos.Entities;
using ApiExpanda.Infrastructure.Modules.Catalogos.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configuraciones de entidades modulares
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public new DbSet<User> Users { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    
    // Módulo Catalogos
    public DbSet<Company> Companies { get; set; }
}
