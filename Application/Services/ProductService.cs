using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Common.Response;
using Core.Entities;
using Core.Interfaces;
using KafkaProducer.Publisher;
using KafkaProducer.Topic;
using Microsoft.Graph.Security.MicrosoftGraphSecurityRunHuntingQuery;
using System.Globalization;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly ITopicPublisher _topicPublisher;
        private readonly IMapper _mapper;


        public ProductService(IProductRepository repository, ITopicPublisher topicPublisher, IMapper mapper)
        {
            _repository = repository;
            _topicPublisher = topicPublisher;
            _mapper = mapper;
        }

        public async Task<List<ProductModel>> GetProductsAsync(Int32 UserId)
        {
            var products = await _repository.GetProductsAsync(UserId);
            // Map Product to ProductModel
            var productModels = _mapper.Map<List<ProductModel>>(products);
            return productModels;
        }

        public async Task<ServiceResponse<ProductModel>> AddProductServiceAsync(ProductModel productModel)
        {
            // Map ProductInput to Product 
            var product = _mapper.Map<Product>(productModel);

            // 🔹 Save product to database
            var savedProduct = await _repository.AddProductAsync(product);


            //        bool isPublished = await _topicPublisher.TryPublishMessage(
            //    TopicNameEnum.AddProductTopic,
            //    savedProduct.Id,
            //    savedProduct
            //);
            await _topicPublisher.TryPublishMessage(
            TopicNameEnum.AddProductTopic,
            savedProduct.Id,
            product);

            var result = _mapper.Map<ProductModel>(savedProduct);

            //Console.WriteLine(isPublished ? "✅ Published" : "❌ Failed");
            if (result == null)
            {
                return new ServiceResponse<ProductModel>
                {
                    Data = null,
                    Message = "Failed to insert product.",
                    Success = false
                };
            }

            return new ServiceResponse<ProductModel>
            {
                Data = result,
                Message = "Product inserted successfully.",
                Success = true
            };
        }
        public async Task<ServiceResponse<UpdateProductModel>> UpdateProductServiceAsync(UpdateProductModel productModel)
        {
            try
            {
                if (productModel.Id <= 0)
                {
                    return new ServiceResponse<UpdateProductModel>
                    {
                        Success = false,
                        Message = "Product ID is required.",
                        Data = null
                    };
                }

                // Call the repository to update and get the updated product
                var updatedProduct = await _repository.UpdateProductRepositoryAsync(_mapper.Map<Product>(productModel));

                if (updatedProduct == null)
                {
                    return new ServiceResponse<UpdateProductModel>
                    {
                        Success = false,
                        Message = "Product not found or update failed.",
                        Data = null
                    };
                }

                return new ServiceResponse<UpdateProductModel>
                {
                    Success = true,
                    Message = "Product updated successfully.",
                    Data = _mapper.Map<UpdateProductModel>(updatedProduct)
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<UpdateProductModel>
                {
                    Success = false,
                    Message = $"Failed to update product: {ex.Message}",
                    Data = null
                };
            }
        }



        // Service Layer
        public async Task<ServiceResponse<string>> DeleteProductServiceAsync(int productId)
        {
            try
            {
                if (productId == null)
                {
                    throw new ArgumentException("Product ID cannot be null.");
                }

                // Delete the product using the repository
                var deletedProduct = await _repository.DeleteProductAsync(productId);

                if (deletedProduct == null)
                {
                    return new ServiceResponse<string>
                    {
                        Success = false,
                        Message = "Product not found or already deleted.",
                        Data = null
                    };
                }


                // Return success response
                return new ServiceResponse<string>
                {
                    Success = true,
                    Message = $"Product '{deletedProduct.Name}' deleted successfully.",
                    Data = deletedProduct.Id.ToString()
                };
            }
            catch (Exception ex)
            {

                // Return error response
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = $"Failed to delete product: {ex.Message}",
                    Data = null
                };
            }
        }







    }
}
