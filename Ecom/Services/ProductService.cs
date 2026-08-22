using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ecom.Data;
using Ecom.Models;
using Ecom.Services.Interfaces;

namespace Ecom.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.Include(p => p.ProductItem).AsNoTracking().ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.ProductItem).AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            var existing = await _context.Products.Include(p => p.ProductItem).FirstOrDefaultAsync(p => p.Id == product.Id);

            if (existing is null)
                return false;

            // Update scalar properties
            existing.ProductName = product.ProductName;
            existing.CreatedBy = product.CreatedBy;
            existing.CreatedOn = product.CreatedOn;
            existing.ModifiedBy = product.ModifiedBy;
            existing.ModifiedOn = product.ModifiedOn;

            // If caller provided ProductItem, update or attach accordingly
            if (product.ProductItem is not null)
            {
                if (existing.ProductItem is null)
                {
                    existing.ProductItem = product.ProductItem;
                }
                else
                {
                    existing.ProductItem.Quantity = product.ProductItem.Quantity;
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Products.FindAsync(id);
            if (existing is null)
                return false;

            _context.Products.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}