using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ecom.Data;
using Ecom.Models;
using Ecom.Services.Interfaces;


namespace Ecom.Services
{
    public class ProductItemService : IProductItemService
    {
        private readonly ApplicationDbContext _context;

        public ProductItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> IncreaseQuantityAsync(int productItemId, int amount)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");

            var item = await _context.ProductItems.FindAsync(productItemId);
            if (item is null) throw new KeyNotFoundException($"ProductItem with id {productItemId} not found.");

            item.Quantity += amount;
            await _context.SaveChangesAsync();

            return item.Quantity;
        }

        public async Task<int> DecreaseQuantityAsync(int productItemId, int amount)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");

            var item = await _context.ProductItems.FindAsync(productItemId);
            if (item is null) throw new KeyNotFoundException($"ProductItem with id {productItemId} not found.");

            var newQty = item.Quantity - amount;
            if (newQty < 0) throw new InvalidOperationException("Cannot decrease quantity below zero.");

            item.Quantity = newQty;
            await _context.SaveChangesAsync();

            return item.Quantity;
        }

        public async Task<int> SetQuantityAsync(int productItemId, int quantity)
        {
            if (quantity < 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity cannot be negative.");

            var item = await _context.ProductItems.FindAsync(productItemId);
            if (item is null) throw new KeyNotFoundException($"ProductItem with id {productItemId} not found.");

            item.Quantity = quantity;
            await _context.SaveChangesAsync();

            return item.Quantity;
        }
    }
}