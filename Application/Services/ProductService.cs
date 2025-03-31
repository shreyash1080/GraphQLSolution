using Application.Models;
using Core.Entities;
using Core.Interfaces;

namespace Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }
        
        public Task<List<Product>> GetProductsAsync()
        {
            var products = _repository.GetProductsAsync();
            return products;
        }

        public async Task<Product>  AddProductServiceAsync(ProductInput productInput)
        {
            var product = new Product
            {
                Name = productInput.Name,
                Price = productInput.Price
            };

            return await _repository.AddProductAsync(product);
        }
    }
}
