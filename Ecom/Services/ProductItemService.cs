using Ecom.Data;
using Ecom.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Ecom.Services
{
    public class ProductItemService : IProductItemService
    {
        private readonly ApplicationDbContext _context;

        public ProductItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> IncreaseQuantityAsync(int productId)
        {
            int amount = 1;
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");

            var item = await _context.ProductItems.FirstOrDefaultAsync(x => x.ProductId == productId);
            if (item is null) throw new KeyNotFoundException($"Product with id {productId} not found.");

            item.Quantity += amount;
            await _context.SaveChangesAsync();

            return item.Quantity;
        }

        public async Task<int> DecreaseQuantityAsync(int productId)
        {
            int amount = 1;
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");

            var item = await _context.ProductItems.FirstOrDefaultAsync(x => x.ProductId == productId);
            if (item is null) throw new KeyNotFoundException($"Product with id {productId} not found.");

            var newQty = item.Quantity - amount;
            if (newQty < 0) throw new InvalidOperationException("Cannot decrease quantity below zero.");

            item.Quantity = newQty;
            await _context.SaveChangesAsync();

            return item.Quantity;
        }

        public async Task<int> SetQuantityAsync(int productId, int quantity)
        {
            if (quantity < 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity cannot be negative.");

            var item = await _context.ProductItems.FirstOrDefaultAsync(x => x.ProductId == productId);
            if (item is null) throw new KeyNotFoundException($"Product with id {productId} not found.");

            item.Quantity = quantity;
            await _context.SaveChangesAsync();

            return item.Quantity;
        }
    }
}