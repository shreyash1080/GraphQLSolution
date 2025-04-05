using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaProducer.Configuration
{
    public class KafkaConfig
    {
        public string BootstrapServers { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public ProducerConfigSettings Producer { get; set; } = new();
        public ConsumerConfigSettings Consumer { get; set; } = new();
    }

    public class ProducerConfigSettings
    {
        public string ClientId { get; set; } = string.Empty;
        public Acks Acks { get; set; } = Acks.All;
        public int MessageTimeoutMs { get; set; } = 30000;
        public int RetryBackoffMs { get; set; } = 1000;
        public bool EnableIdempotence { get; set; } = true;
        public int MaxInFlight { get; set; } = 5;
        public int LingerMs { get; set; } = 5;
        public CompressionType CompressionType { get; set; } = CompressionType.Snappy;
    }

    public class ConsumerConfigSettings
    {
        public string GroupId { get; set; } = string.Empty;
        public string AutoOffsetReset { get; set; } = "Earliest";
    }
}


//provides strongly typed access to Kafka settings.

//Avoids hardcoding values in producer/consumer logic.

//Ensures easier maintainability.