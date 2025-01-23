using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace weatherApp.Models
{
   public class TokenResponse
    {
        [JsonPropertyName("token")]
        public string  Token { get; set; }
        [JsonPropertyName("expiration")]
        public DateTime Expiration { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }


    }
}
