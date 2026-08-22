using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ecom.Models;

namespace Ecom.Services.Interfaces
{
    public interface IProductItemService
    {
        // Quantity-specific operations
        Task<int> IncreaseQuantityAsync(int productItemId, int amount);
        Task<int> DecreaseQuantityAsync(int productItemId, int amount);
        Task<int> SetQuantityAsync(int productItemId, int quantity);
    }
}