using API.GraphQl.Types;
using Application.Models;
using Application.Services;
using Core.Entities;

namespace API.GraphQl
{
    public class Mutation
    {

        public async Task<Product>  AddProductAsync(ProductInput productInput , [Service] ProductService productService)
        {
            return await productService.AddProductServiceAsync(productInput);
        }


        public async Task<Users> AddUsersAsync(UserModel userInput, [Service] ProductService productService)
        {

            return await productService.AddUsersServiceAsync(userInput);
        }

    }
}
