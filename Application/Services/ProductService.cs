using Application.Interfaces;
using Application.Models;
using AutoMapper;
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

        public async Task<List<ProductModel>> GetProductsAsync()
        {
            var products = await _repository.GetProductsAsync();
            // Map Product to ProductModel
            var productModels = _mapper.Map<List<ProductModel>>(products);
            return productModels;
        }

        public async Task<ProductModel> AddProductServiceAsync(ProductModel productModel)
        {
            // Map ProductInput to Product 
            var product = _mapper.Map<Product>(productModel);

            // 🔹 Save product to database
            var savedProduct = await _repository.AddProductAsync(product);

            // 🔹 Debugging: Log the saved product ID
            Console.WriteLine($"✅ Product inserted: {savedProduct.Id} - {savedProduct.Name}");

            //        bool isPublished = await _topicPublisher.TryPublishMessage(
            //    TopicNameEnum.AddProductTopic,
            //    savedProduct.Id,
            //    savedProduct
            //);
            await _topicPublisher.TryPublishMessage(
            TopicNameEnum.AddProductTopic,
            savedProduct.Id,
            product);

            var result = _mapper.Map<ProductModel>(product);

            //Console.WriteLine(isPublished ? "✅ Published" : "❌ Failed");

            return result;
        }

    }
}
