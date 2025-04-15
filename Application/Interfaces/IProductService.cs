using Application.Models;
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
        Task<ProductModel> AddProductServiceAsync(ProductModel productInput);
    }
}
