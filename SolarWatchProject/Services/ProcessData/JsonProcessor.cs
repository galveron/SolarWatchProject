using SolarWatchProjectBackend.Models;
using System.Text.Json;

namespace SolarWatchProjectBackend.Services.ProcessData
{
    public class JsonProcessor : IJsonProcessor
    {
        public City GetProcessedGeoData(string geoData)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("bármi: " + JsonDocument.Parse(geoData).RootElement.ToString());
                var result = JsonDocument.Parse(geoData).RootElement[0];

                var cityData = new City()
                {
                    Name = result.GetProperty("name").GetString(),
                    Latitude = result.GetProperty("lat").GetDouble(),
                    Longitude = result.GetProperty("lon").GetDouble(),
                    Country = result.GetProperty("country").GetString()
                };

                return cityData;
            }
            catch (Exception e)
            {
                throw new Exception("Error parsing geo data");
            }
        }

        public SunRiseAndSetTime GetProcessedSunData(string sunData, City city, DateTime date)
        {
            try
            {
                var result = JsonDocument.Parse(sunData).RootElement.GetProperty("results");

                var forecast = new SunRiseAndSetTime
                {
                    Date = date,
                    SunRise = $"Sunrise: {result.GetProperty("sunrise").GetString()}",
                    SunSet = $"Sunset: {result.GetProperty("sunset").GetString()}",
                    City = city.Name,
                    Description = ""
                };

                return forecast;
            }
            catch
            {
                throw new Exception("Error parsing sun data");
            }
        }
    }
}
