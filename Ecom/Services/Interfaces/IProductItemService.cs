using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ecom.Models;

namespace Ecom.Services.Interfaces
{
    public interface IProductItemService
    {
        // Quantity-specific operations
        Task<int> IncreaseQuantityAsync(int productId);
        Task<int> DecreaseQuantityAsync(int productId);
        Task<int> SetQuantityAsync(int productId, int quantity);
    }
}