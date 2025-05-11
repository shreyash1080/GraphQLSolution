using API.GraphQl.Types;
using Application.Interfaces;
using Application.Models;
using Application.Services;
using Common.Response;
using Core.Entities;
using HotChocolate.Authorization;

namespace API.GraphQl
{
    public class Mutation
    {
        [Authorize]
        public async Task<ServiceResponse<ProductModel>> AddProductAsync(ProductModel productModel , [Service] ProductService productService)
        {
            return await productService.AddProductServiceAsync(productModel);
        }

        [Authorize]
        public async Task<QLResponseUserModel> AddUsersAsync(UserModel userInput, [Service] UserService userService)
        {

            return await userService.AddUsersServiceAsync(userInput);
        }

        [Authorize]
        public async Task<ServiceResponse<string>> DeleteProductAsync(int productId, [Service] ProductService productService)
        {
            var result = await productService.DeleteProductServiceAsync(productId);
            return result;
        }

        [Authorize]
        public async Task<ServiceResponse<UpdateProductModel>> UpdateProductAsync(UpdateProductModel UpdateProductModel, [Service] ProductService productService)
        {
            return await productService.UpdateProductServiceAsync(UpdateProductModel);
        }
    }
}
