using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherApp.Shared
{
    public class IpAddressResponse
    {
        [JsonPropertyName("ip")]
        public string? Ip { get; set; }
    }
}
