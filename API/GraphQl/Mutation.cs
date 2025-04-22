using API.GraphQl.Types;
using Application.Interfaces;
using Application.Models;
using Application.Services;
using Common.Response;
using Core.Entities;

namespace API.GraphQl
{
    public class Mutation
    {


        public async Task<ServiceResponse<ProductModel>> AddProductAsync(ProductModel productModel , [Service] ProductService productService)
        {
            return await productService.AddProductServiceAsync(productModel);
        }


        public async Task<UserModel> AddUsersAsync(UserModel userInput, [Service] UserService userService)
        {

            return await userService.AddUsersServiceAsync(userInput);
        }


        public async Task<ServiceResponse<string>> DeleteProductAsync(int productId, [Service] ProductService productService)
        {
            var result = await productService.DeleteProductServiceAsync(productId);
            return result;
        }

        public async Task<ServiceResponse<UpdateProductModel>> UpdateProductAsync(UpdateProductModel UpdateProductModel, [Service] ProductService productService)
        {
            return await productService.UpdateProductServiceAsync(UpdateProductModel);
        }

    }
}
