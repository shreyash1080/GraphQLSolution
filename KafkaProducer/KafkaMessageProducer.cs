using Confluent.Kafka;

namespace KafkaProducer
{
    public class KafkaMessageProducer
    {
        private readonly string _bootstrapServers;
        private readonly string _topic;

        public KafkaMessageProducer(string bootstrapServers, string topic)
        {
            _bootstrapServers = bootstrapServers;// _bootstrapServers - It is the Kafka broker address (localhost:9092 in local setups).
            _topic = topic;
        }

        public async Task SendMessageAsync(string message)
        {
            // Configure the Kafka producer
            var config = new ProducerConfig
            {
                BootstrapServers = _bootstrapServers
            };

            // Using statement ensures proper disposal of the producer
            using var producer = new ProducerBuilder<Null, string>(config).Build();
            //Kafka uses keys to determine which partition a message should be stored in.
            //If you set Null, Kafka will randomly distribute messages across partitions.

            try
            {
                // Send message to Kafka
                var deliveryResult = await producer.ProduceAsync(_topic, new Message<Null, string> { Value = message });

                // Log success
                Console.WriteLine($"Message sent to {deliveryResult.TopicPartitionOffset}: {message}");
            }
            catch (ProduceException<Null, string> e)
            {
                // Handle error
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }
        }
    }
}
