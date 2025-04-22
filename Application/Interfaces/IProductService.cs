using Application.Models;
using Common.Response;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductService
    {

        Task<List<ProductModel>> GetProductsAsync();
        Task<ServiceResponse<ProductModel>> AddProductServiceAsync(ProductModel productInput);

        Task<ServiceResponse<string>> DeleteProductServiceAsync(int productId);

        // Task<bool> PublishProductAsync(Product product);
        Task<ServiceResponse<UpdateProductModel>> UpdateProductServiceAsync(UpdateProductModel productModel);
    }
}
