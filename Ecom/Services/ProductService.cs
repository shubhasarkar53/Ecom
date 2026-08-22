using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ecom.Data;
using Ecom.Models;
using Ecom.Services.Interfaces;
using Ecom.DTOs.Product;

namespace Ecom.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetAllAsync()
        {
            var products = await _context.Products.Include(p => p.ProductItem).AsNoTracking().ToListAsync();

            return products.Select(p => ToResponseDTO(p));
        }

        public async Task<ProductResponseDTO?> GetByIdAsync(int id)
        {
            var product = await _context.Products.Include(p => p.ProductItem).AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            return product is null ? null : ToResponseDTO(product);
        }

        public async Task<ProductResponseDTO> CreateAsync(ProductRequestDTO reqProduct)
        {
            Product product = new Product
            {
                ProductName = reqProduct.ProductName,
                CreatedBy = "System",//implement later
                CreatedOn = DateTime.UtcNow,
                ProductItem = new ProductItem
                {
                    Quantity = reqProduct.Quantity
                }
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return new ProductResponseDTO
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Quantity = product.ProductItem.Quantity,
                CreatedBy = product.CreatedBy,
                CreatedOn = product.CreatedOn
            };
        }

        public async Task<bool> UpdateAsync(ProductRequestDTO reqProduct, int id)
        {
            var existing = await _context.Products.Include(p => p.ProductItem).FirstOrDefaultAsync(p => p.Id == id);

            if (existing is null)
                return false;

            // Update scalar properties
            existing.ProductName = reqProduct.ProductName;
            existing.ModifiedBy = "System";//implement later
            existing.ModifiedOn = DateTime.UtcNow;

            // If caller provided ProductItem, update or attach accordingly
            if (existing.ProductItem is null)
            {
                existing.ProductItem = new ProductItem { Quantity = reqProduct.Quantity };
            }
            else
            {
                existing.ProductItem.Quantity = reqProduct.Quantity;
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

        private static ProductResponseDTO ToResponseDTO(Product product) => new()
        {
            Id = product.Id,
            ProductName = product.ProductName,
            Quantity = product.ProductItem?.Quantity ?? 0,
            CreatedBy = product.CreatedBy,
            CreatedOn = product.CreatedOn
        };
    }
}