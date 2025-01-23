using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weatherApp.Services
{
    public class APIResponse<T>
    {
        
        public string Message { get; set; }
        public T Data { get; set; }
        public bool HasError => !string.IsNullOrEmpty(Message);
    }
}
