using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherApp.Shared
{
    public class CacheData
    {
        [JsonPropertyName("json")]
        public string? Json { get; set; }
        [JsonPropertyName("currentTime")]
        public DateTime CurrentTime { get; set; }
        [JsonPropertyName("forcastWeather")]
        public string? ForecastWeatherJson { get; set; }
    }
}
