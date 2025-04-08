using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaProducer.Models
{
  
        public record UserKey(int UserId);
        public record UserEvent(string Name, string Email);
 

}
