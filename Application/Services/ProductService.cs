using Application.Models;
using Core.Entities;
using Core.Interfaces;
using KafkaProducer.Publisher;
using KafkaProducer.Topic;

namespace Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repository;
        private readonly ITopicPublisher _topicPublisher;


        public ProductService(IProductRepository repository, ITopicPublisher topicPublisher)
        {
            _repository = repository;
            _topicPublisher = topicPublisher;
        }
        
        public Task<List<Product>> GetProductsAsync()
        {
            var products = _repository.GetProductsAsync();
            return products;
        }

        public async Task<Product>  AddProductServiceAsync(ProductInput productInput)
        {
            var product = new Product
            {
                Name = productInput.Name,
                Price = productInput.Price
            };

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

            //Console.WriteLine(isPublished ? "✅ Published" : "❌ Failed");

            return savedProduct;
        }

        public async Task<Users> AddUsersServiceAsync(UserModel user)
        {
            var userEntity = new Users
            {
                email = user.Email,
                first_name = user.FirstName,
                last_name = user.LastName,
                password_hash = user.Password,
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow,
            };

            var users = await _repository.AddUsersAsync(userEntity);
            return users;
        }

    }
}
