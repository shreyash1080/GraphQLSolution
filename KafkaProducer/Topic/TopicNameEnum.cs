using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaProducer.Topic
{
    public enum TopicNameEnum
    {
        [Description("Sample")]
        None = 0,

        [Description("add.product.topic.evt")]
        AddProductTopic = 1,

        [Description("update.product.topic.evt.")]
        UpdateProductTopic = 2,

        [Description("delete.product.topic.evt.")]
        DeleteProductTopic = 3
    }

    public static class TopicNameEnumExtensions
    {
        /// <summary>
        /// Retrieves the string description of the given topic enum.
        /// </summary>
        /// <param name="topic">The topic enum value.</param>
        /// <returns>The corresponding string description.</returns>
        public static string GetEnumDescription(this TopicNameEnum topic)
        {
            var fieldInfo = topic.GetType().GetField(topic.ToString());
            if (fieldInfo == null)
            {
                return topic.ToString();
            }

            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : topic.ToString();
        }
    }
}
