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
        public string ClientId { get; set; } = "ccloud-csharp-client-1fd89a7b-0e38-407c-b0c4-28100baaa739";
        public Acks Acks { get; set; } = Acks.All;
        public int MessageTimeoutMs { get; set; } = 30000;
        public int RetryBackoffMs { get; set; } = 1000;
        public bool EnableIdempotence { get; set; } = true;

        // Add these new properties for future appsettings use
        public string SecurityProtocol { get; set; } = "SaslSsl";
        public string SaslMechanism { get; set; } = "Plain";
        public string SaslUsername { get; set; } = string.Empty;
        public string SaslPassword { get; set; } = string.Empty;
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