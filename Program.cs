using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Enter the city for weather information:");
        string city = Console.ReadLine();

        if (!string.IsNullOrEmpty(city))
        {
            try
            {
                var weatherData = await GetWeatherData(city);

                Console.WriteLine("Temperature: {0:0.2}°C", weatherData.Temp);
                Console.WriteLine("Description: {0}", weatherData.Description);
                Console.WriteLine("Wind speed: {0:0.1} m/s", weatherData.Wind.Speed);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
        }
        else
        {
            Console.WriteLine("City name cannot be empty");
        }
    }

    static async Task<WeatherData> GetWeatherData(string city)
    {
        try
        {
            var requestUrl = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid=c4da20a20e511c0b7466479ae33f29aa&units=metric";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var weatherData = JsonSerializer.Deserialize<WeatherData>(json);
                    return weatherData;
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode}");
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error fetching weather data", ex);
        }
    }
}

class WeatherData
{
    public double Temp { get; set; }
    public string Description { get; set; }
    public WindData Wind { get; set; }
}

class WindData
{
    public double Speed { get; set; }
}