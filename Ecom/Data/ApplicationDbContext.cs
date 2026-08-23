using Ecom.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Product
            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductItem)
                .WithOne(pi => pi.Product)
                .HasForeignKey<ProductItem>(pi => pi.ProductId);

            // User configuration
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasMaxLength(150)
                .IsRequired();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
