using System;
using System.Threading.Tasks;
using KafkaProducer.Topic; // Assuming TopicNameEnum is inside an Enum folder

namespace KafkaProducer.Publisher
{
    /// <summary>
    /// Defines a contract for publishing messages to Kafka topics.
    /// </summary>
    public interface ITopicPublisher : IDisposable
    {
        /// <summary>
        /// Publishes a message to the specified Kafka topic.
        /// </summary>
        /// <typeparam name="TKey">The type of the key used for partitioning.</typeparam>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="topicName">The Kafka topic enum.</param>
        /// <param name="key">The key for partitioning.</param>
        /// <param name="message">The message to publish.</param>
        /// <returns>A Task representing the asynchronous operation. Returns true if the message is successfully published.</returns>
        Task<bool> TryPublishMessage<TKey, TMessage>(TopicNameEnum topicName, TKey key, TMessage message);
    }
}
