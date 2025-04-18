using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Response
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; } // Indicates if the operation was successful
        public string Message { get; set; } // A message for success or error details
        public T Data { get; set; } // The data or result of the operation
    }

}
