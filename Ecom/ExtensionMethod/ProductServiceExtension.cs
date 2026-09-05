using Ecom.Models;
using Ecom.Services;
using Ecom.Services.Interfaces;

namespace Ecom.ExtensionMethod
{
    public static class ProductServiceExtension
    {
        public static IServiceCollection AddProductServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductItemService, ProductItemService>();

            return services;
        }
    }
}
