using Confluent.SchemaRegistry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaProducer.SchemaRegistrar
{
    public static class SchemaRegistrar
    {
        public static async Task<int> RegisterSchemaAsync(string schemaPath, string subject, ISchemaRegistryClient registryClient)
        {
            var schemaString = await File.ReadAllTextAsync(schemaPath);
            return await registryClient.RegisterSchemaAsync(subject, new Confluent.SchemaRegistry.Schema(schemaString, Confluent.SchemaRegistry.SchemaType.Avro));
        }
    }
}
