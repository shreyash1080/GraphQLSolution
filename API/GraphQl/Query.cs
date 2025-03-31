using Application.Services; // Now valid because Query is in API or Application
using Core.Entities;
using HotChocolate;

namespace API.GraphQL // Update based on where you moved it
{
    public class Query
    {
        private readonly ProductService _productService;

        public Query(ProductService productService)// // Inject ProductService in constructor
        {
            _productService = productService;
        }

        // Define a GraphQL query to fetch products
        [GraphQLName("products")]
        // Call the ProductService method to get the list of products
        public async Task<List<Product>> GetProductsAsync() => await _productService.GetProductsAsync();


        //If No Constructor is ther ethen we use this service -- Here, HotChocolate takes care of injecting ProductService at runtime.
        //public async Task<List<Product>> GetProductsAsync([Service] ProductService productService)
        //{
        //    return await productService.GetProductsAsync();
        //}
    }
}
