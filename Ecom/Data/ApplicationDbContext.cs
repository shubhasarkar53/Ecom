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

            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductItem)
                .WithOne(pi => pi.Product)
                .HasForeignKey<ProductItem>(pi => pi.ProductId);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
    }
}
