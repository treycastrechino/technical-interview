using WeatherApp.BusinessLogic.IBusinessLogic;
using WeatherApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System.Text.RegularExpressions;
using Amazon.Runtime.Internal.Util;
using System.Net.Http;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using log4net;

namespace WeatherApp.BusinessLogic.BusinessLogic
{
    public class HomeService : IHome
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(HomeService));
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private static Dictionary<string, CacheData> _cache = new Dictionary<string, CacheData>();
        private static string RootPath = string.Empty;
        public HomeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            LoadCacheFromFile();
            _apiKey = LoadApiKey();
        }

        public async Task<WeatherModel> GetWeatherInfo(string CityName)
        {
            string result = string.Empty;
            try
            {
                if (!ValidateCityName(CityName))
                {
                    return new WeatherModel
                    {
                        Json = CityName,
                        ForecastJson = CityName,
                        Message = "City name can only contain letters and spaces.",
                        Status = "failed"

                    };
                }
                else
                {
                    string weatherDataJson = await GetWeatherDataAsync(CityName);
                    string weatherForecastDataJson = await GetForecastDataAsync(CityName);
                    return new WeatherModel
                    {
                        Json = weatherDataJson,
                        ForecastJson = weatherForecastDataJson,
                        Message = "Weather data fetched successfully",
                        Status = "success"

                    };
                }

            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<List<LogModel>> GetLogsInfo()
        {
            List<LogModel> logModels = new List<LogModel>();
            try
            {
                string logFilePath = Path.Combine(RootPath, "Logs");

                string[] files = Directory.GetFiles(logFilePath);


                foreach (string file in files)
                {

                    if (File.Exists(file))
                    {
                        var lines = await File.ReadAllLinesAsync(file);

                        foreach (var line in lines)
                        {
                            var logEntry = ParseLogEntry(line);
                            if (logEntry != null)
                            {
                                logModels.Add(logEntry);
                            }
                        }
                    }
                }
                return logModels;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading log file: {ex.Message}");
                return new List<LogModel>();
            }
        }

        private LogModel? ParseLogEntry(string logLine)
        {
            var parts = logLine.Split(new[] { ' ' }, 4, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 4) return null;

            return new LogModel
            {
                Timestamp = $"{parts[0]} {parts[1]}",
                Level = parts[2].Trim('[', ']'),
                Message = parts[3]
            };
        }

        private string LoadApiKey()
        {
            try
            {
                string directoryPath = Environment.CurrentDirectory;
                RootPath = directoryPath;
                string configPath = Path.Combine(Path.Combine(RootPath, "Configuration"), "config.json");

                if (!File.Exists(configPath))
                {
                    log.Info($"Could not find config file at: {configPath}.");
                    throw new FileNotFoundException($"Could not find config file at: {configPath}.");
                }

                var config = JObject.Parse(File.ReadAllText(configPath));
                log.Info($"API Key loaded successfully from {configPath}.");
                return config["ApiKey"]?.ToString() ?? throw new Exception("API Key not found.");
            }
            catch (Exception ex)
            {
                log.Error("Error loading API key.", ex);
                throw;
            }
        }
        public async Task<string> GetWeatherDataAsync(string city)
        {
            
            if (_cache.TryGetValue(city.ToLower().Trim(), out var cachedData))
            {
                Console.Clear();
                if ((DateTime.UtcNow - cachedData.CurrentTime).TotalMinutes < 60)
                {
                    Console.WriteLine("Using cached data...");
                    return cachedData.Json;
                }
            }

            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=imperial";
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    log.Error("Unauthorized access - Invalid API key.");
                    throw new UnauthorizedAccessException("Invalid API key or unauthorized access.");
                }
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    log.Warn($"City '{city}' not found.");
                    throw new KeyNotFoundException($"City '{city}' not found. Please check the city name and try again.");
                }
                response.EnsureSuccessStatusCode();
                log.Info($"Fetched weather data for city: {city}");
                string json = await response.Content.ReadAsStringAsync();
                CacheData(city.ToLower().Trim(), json);

                return json;
            }
            catch (HttpRequestException ex) when (ex.InnerException is System.Net.Sockets.SocketException)
            {
                log.Error($"Network error while fetching data for {city}. Fallback to cache.", ex);
                return FallbackToCache(city, "Network error: Unable to reach the weather service. Please check your internet connection.");
            }
            catch (HttpRequestException ex) when (ex.InnerException is TaskCanceledException)
            {
                log.Error($"Request timed out for {city}. Fallback to cache.", ex);
                return FallbackToCache(city, "Request timed out. The server may be down or experiencing high load. Try again later.");
            }
            catch (HttpRequestException ex)
            {
                log.Error($"Error fetching weather data for {city}. Fallback to cache.", ex);
                return FallbackToCache(city, "An error occurred while fetching weather data. Please try again later.");
            }

            catch (UnauthorizedAccessException ex)
            {
                log.Error("Unauthorized access.", ex);
                throw new UnauthorizedAccessException("Invalid API key or unauthorized access.");
            }
            catch (KeyNotFoundException ex)
            {
                log.Error($"City '{city}' not found.", ex);
                throw;
            }
        }
        public async Task<string> GetForecastDataAsync(string city)
        {
            

            if (_cache.TryGetValue(city.ToLower().Trim(), out var cachedData))
            {
                if (!string.IsNullOrWhiteSpace(cachedData.ForecastWeatherJson) && (DateTime.UtcNow - cachedData.CurrentTime).TotalMinutes < 60)
                {
                    Console.WriteLine("Using cached forecast data...");
                    return cachedData.ForecastWeatherJson;
                }
            }

            string url = $"https://api.openweathermap.org/data/2.5/forecast?q={city}&appid={_apiKey}&units=imperial&cnt=40";
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    log.Error("Unauthorized access - Invalid API key.");
                    throw new UnauthorizedAccessException("Invalid API key or unauthorized access.");
                }
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    log.Warn($"City '{city}' not found.");
                    throw new KeyNotFoundException($"City '{city}' not found. Please check the city name and try again.");
                }
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                log.Info($"Fetched forecast data for city: {city}");
                if (_cache.TryGetValue(city.ToLower().Trim(), out var existingData))
                {
                    existingData.ForecastWeatherJson = json;
                    existingData.CurrentTime = DateTime.UtcNow;
                }
                else
                {
                    _cache[city.ToLower().Trim()] = new CacheData
                    {
                        Json = string.Empty,
                        ForecastWeatherJson = json,
                        CurrentTime = DateTime.UtcNow
                    };
                }

                SaveCacheToFile();
                log.Info("Weather Forecast data cached successfully.");
                return json;
            }
            catch (Exception ex)
            {
                log.Error($"Error fetching forecast data for {city}. Fallback to cache.", ex);
                return FallbackToCache(city, "An error occurred while fetching forecast data. Please try again later.");
            }
        }
        private string FallbackToCache(string city, string errorMessage)
        {
            log.Warn($"{errorMessage} Using last cached data.");
            if (_cache.TryGetValue(city.ToLower(), out var cachedData))
            {
                Console.WriteLine($"{errorMessage} Using last cached data.");
                return cachedData.Json;
            }
            throw new Exception($"{errorMessage} No cached data available.");
        }
        private void CacheData(string city, string data, string forecastData = null)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                log.Warn($"Attempted to cache null or empty data for city: {city}");
                Console.WriteLine("Warning: Attempted to cache null or empty data.");
                return;
            }

            if (!_cache.ContainsKey(city) || (DateTime.UtcNow - _cache[city].CurrentTime).TotalMinutes >= 60)
            {
                _cache[city] = new CacheData
                {
                    Json = data,
                    ForecastWeatherJson = forecastData,
                    CurrentTime = DateTime.UtcNow
                };
                SaveCacheToFile();
                log.Info("Weather data cached successfully.");
            }
        }
        private void LoadCacheFromFile()
        {
            try
            {
                string CacheFilePath = Path.Combine(Path.Combine(RootPath, "Configuration"), "weatherCache.json");
                if (!File.Exists(CacheFilePath))
                {
                    log.Warn($"Cache file not found at: {CacheFilePath}");
                    return;
                }
                string json = File.ReadAllText(CacheFilePath);
                _cache = JsonSerializer.Deserialize<Dictionary<string, CacheData>>(json) ?? new Dictionary<string, CacheData>();
                log.Info("Cache loaded successfully.");
            }
            catch (Exception ex)
            {
                log.Error("Error loading cache.", ex);
            }
        }
        private void SaveCacheToFile()
        {
            try
            {
                string CacheFilePath = Path.Combine(Path.Combine(RootPath, "Configuration"), "weatherCache.json");
                if (!File.Exists(CacheFilePath))
                {
                    throw new FileNotFoundException($"Could not find cache file at: {CacheFilePath}");
                }
                string json = JsonSerializer.Serialize(_cache);
                File.WriteAllText(CacheFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving cache: {ex.Message}");
                log.Error("Error saving cache to file.", ex);
            }
        }
        public bool ValidateCityName(string cityName)
        {
            if (!Regex.IsMatch(cityName, @"^[a-zA-Z\s]+$")) return false;
            else return true;
        }

    }
}
