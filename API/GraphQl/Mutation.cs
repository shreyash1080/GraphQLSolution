using API.GraphQl.Types;
using Application.Models;
using Application.Services;
using Core.Entities;

namespace API.GraphQl
{
    public class Mutation
    {

        public async Task<ProductModel>  AddProductAsync(ProductModel productModel , [Service] ProductService productService)
        {
            return await productService.AddProductServiceAsync(productModel);
        }


        public async Task<UserModel> AddUsersAsync(UserModel userInput, [Service] UserService userService)
        {

            return await userService.AddUsersServiceAsync(userInput);
        }

    }
}
