using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq; // Додано простір імен для JObject

public class LocationWeather
{
    private static readonly HttpClient client = new HttpClient();

    public async Task<string> GetLocationAsync()
    {
        var response = await client.GetStringAsync("http://ip-api.com/json/");
        var locationData = JObject.Parse(response);
        return $"{locationData["city"]}, {locationData["country"]}";
    }

    public async Task<string> GetWeatherAsync(string location)
    {
        string apiKey = "cd72eebe286347109c6231838251702"; // Вставте ваш API ключ для сервісу погоди
        var response = await client.GetStringAsync($"http://api.weatherapi.com/v1/current.json?key={apiKey}&q={location}");
        var weatherData = JObject.Parse(response);
        var temp_c = weatherData["current"]?["temp_c"]?.ToString() ?? "N/A";
        var conditionText = weatherData["current"]?["condition"]?["text"]?.ToString() ?? "N/A";
        return $"{temp_c}°C, {conditionText}";
    }

    public string GetCurrentTime()
    {
        return DateTime.Now.ToString("HH:mm:ss");
    }

    public async Task<string> GetLocationWeatherAsync()
    {
        string location = await GetLocationAsync();
        string weather = await GetWeatherAsync(location);
        string time = GetCurrentTime();
        return $"Місце: {location}\nЧас: {time}\nПогода: {weather}";
    }

    public async Task RunAsync()
    {
        await Task.Run(async () =>
        {
            while (true)
            {
                Console.Clear();

                string locationWeather = await GetLocationWeatherAsync();
                Console.WriteLine(locationWeather);
                Console.WriteLine("------------------------");
                await Task.Delay(10000); // Затримка на 10 секунд перед наступним оновленням
            }
        });

        while (true)
        {
            string? input = Console.ReadLine();
            if (input?.ToLower() == "exit")
            {
                break;
            }
        }
    }
}