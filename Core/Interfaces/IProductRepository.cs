using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAsync();

        Task<Product> AddProductAsync(Product product);
        Task<Product> DeleteProductAsync(int productId);
        Task<Product> UpdateProductRepositoryAsync(Product productModel);
    }

}
