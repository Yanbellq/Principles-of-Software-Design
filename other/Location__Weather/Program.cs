using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        LocationWeather locationWeather = new LocationWeather();
        await locationWeather.RunAsync();
    }
}