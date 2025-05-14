using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using AutoMapper;
using Core.Entities;

namespace Application.AutoMapping
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<ProductModel, Product>();
            CreateMap<Product, ProductModel>();

            CreateMap<UpdateProductModel, Product>();
            CreateMap<Product, UpdateProductModel>();
        }
    }
}
