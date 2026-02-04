using Microsoft.Maui.Controls;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using static System.Net.WebRequestMethods;

namespace ElorMAUI.Services
{
    public class WeatherService
    {
        private readonly HttpClient _http;
        public WeatherService(HttpClient http) => _http = http;

        public async Task<WeatherData?> GetWeatherAsync(double lat, double lon)
        {
            try
            {
                // InvariantCulture evita errores si tu PC usa comas en vez de puntos para decimales
                string sLat = lat.ToString(CultureInfo.InvariantCulture);
                string sLon = lon.ToString(CultureInfo.InvariantCulture);

                var url = $"https://api.open-meteo.com/v1/forecast?latitude={sLat}&longitude={sLon}&current_weather=true&daily=temperature_2m_max,temperature_2m_min&timezone=auto";

                return await _http.GetFromJsonAsync<WeatherData>(url);
            }
            catch { return null; }
        }


    }

    // Esta es la clase principal que recibe los datos
    public class WeatherData
    {
        [JsonPropertyName("current_weather")]
        public CurrentWeather Current { get; set; } = new();

        [JsonPropertyName("daily")]
        public DailyForecast Daily { get; set; } = new(); // Esto es lo que faltaba
    }

    public class CurrentWeather
    {
        // Open-Meteo lo envía como "temperature"
        [JsonPropertyName("temperature")]
        public double Temperature { get; set; }
    }

    public class DailyForecast
    {
        [JsonPropertyName("time")]
        public List<string> Days { get; set; } = new();

        [JsonPropertyName("temperature_2m_max")]
        public List<double> MaxTemps { get; set; } = new(); // Asegúrate de que se llame así

        [JsonPropertyName("temperature_2m_min")]
        public List<double> MinTemps { get; set; } = new();
    }
}