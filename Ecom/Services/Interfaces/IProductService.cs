using Ecom.DTOs.Product;
using Ecom.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ecom.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDTO>> GetAllAsync();
        Task<ProductResponseDTO?> GetByIdAsync(int id);
        Task<ProductResponseDTO> CreateAsync(ProductRequestDTO req);
        Task<bool> UpdateAsync(ProductRequestDTO req, int id);
        Task<bool> DeleteAsync(int id);
    }
}