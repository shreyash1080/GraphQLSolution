using Application.Models;
using Application.Services; // Now valid because Query is in API or Application
using Core.Entities;
using HotChocolate;
using Microsoft.Graph.Models;

namespace API.GraphQL // Update based on where you moved it
{
    public class Query
    {
        private readonly ProductService _productService;
        private readonly UserService _userService;

        public Query(ProductService productService, UserService userService)// // Inject ProductService in constructor
        {
            _productService = productService;
            _userService = userService;
        }

        // Define a GraphQL query to fetch products
        [GraphQLName("GetProductsAsync")]
        // Call the ProductService method to get the list of products
        public async Task<List<ProductModel>> GetProductsAsync() => await _productService.GetProductsAsync();


        [GraphQLName("GetUserLoginAsync")]
        public async Task<UserModel> GetUserLoginAsync(string email, string password)
        {
            return await _userService.GetUserLoginServiceAsync(email, password);
        }

        //If No Constructor is ther ethen we use this service -- Here, HotChocolate takes care of injecting ProductService at runtime.
        //pzublic async Task<List<Product>> GetProductsAsync([Service] ProductService productService)
        //{
        //    return await productService.GetProductsAsync();
        //}

    
    }
}
