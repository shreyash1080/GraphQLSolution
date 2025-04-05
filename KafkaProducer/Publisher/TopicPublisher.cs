using Confluent.Kafka;
using KafkaProducer.Configuration;
using KafkaProducer.Topic;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Text.Json;

namespace KafkaProducer.Publisher
{
    public class TopicPublisher : ITopicPublisher
    {
        private readonly IProducer<string, string> _producer;
        private readonly KafkaConfig _kafkaConfig;
        private readonly ILogger<TopicPublisher> _logger;

        /// <summary>
        /// Initializes the TopicPublisher with Kafka configurations and logger.
        /// </summary>
        /// <param name="kafkaConfig">Configuration settings for Kafka.</param>
        /// <param name="logger">Logger instance for logging messages.</param>
        public TopicPublisher(KafkaConfig kafkaConfig, ILogger<TopicPublisher> logger)
        {
            _kafkaConfig = kafkaConfig;
            _logger = logger;

            // Test broker connectivity at startup
            //using var adminClient = new AdminClientBuilder(new AdminClientConfig
            //{ BootstrapServers = kafkaConfig.BootstrapServers }).Build();
            //var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
            //if (metadata.Brokers.Count == 0)
            //{
            //    throw new InvalidOperationException("Kafka broker unreachable");
            //}

            //ProducerConfig is a configuration class for setting up a Kafka producer. It defines how the producer behaves, including Kafka broker settings, acknowledgments, and client identity.
            var config = new ProducerConfig
            {
                BootstrapServers = kafkaConfig.BootstrapServers,// Kafka broker address
                ClientId = kafkaConfig.Producer.ClientId,// Unique identifier for this producer
                Acks = kafkaConfig.Producer.Acks,// Controls message durability
                MessageTimeoutMs = kafkaConfig.Producer.MessageTimeoutMs, // Add
                RetryBackoffMs = kafkaConfig.Producer.RetryBackoffMs,      // Add
                EnableIdempotence = kafkaConfig.Producer.EnableIdempotence // Add
            };

            //A builder class that helps create a Kafka producer based on ProducerConfig.
            _producer = new ProducerBuilder<string, string>(config)
                .SetStatisticsHandler((_, json) =>
                    _logger.LogInformation($"Producer stats: {json}"))
                .Build();
        }

        public async Task<bool> TryPublishMessage<TKey, TMessage>(TopicNameEnum topicName, TKey key, TMessage message)
        {


            try
            {
                Console.WriteLine($"🔹 Publishing Kafka Message - Topic: {topicName.GetEnumDescription()}, Key: {key}");

                // Convert the message and key to string for Kafka.
                var kafkaMessage = new Message<string, string>
                {
                    Key = key?.ToString(),
                    Value = JsonSerializer.Serialize(message) // Use System.Text.Json
                };

                // Publish message to the Kafka topic
                //An asynchronous method that sends messages to a Kafka topic.
                var result = await _producer.ProduceAsync(topicName.GetEnumDescription(), kafkaMessage);

                _logger.LogInformation($"Message sent to {result.TopicPartitionOffset}: {message}");

                Console.WriteLine($"✅ Kafka Message Sent - Offset: {result.Offset}");

                return true;
            }
            catch (ProduceException<string, string> ex)
            {
                _logger.LogError($"Kafka delivery error: {ex.Error.Reason}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error while publishing message: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Properly disposes the Kafka producer when no longer needed.
        /// </summary>
        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(10)); // Ensures all messages are sent before closing the producer. Prevents message loss.
                        _producer.Dispose(); // Release Kafka producer resources
        }
    }
}

