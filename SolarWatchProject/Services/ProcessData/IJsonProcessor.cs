using SolarWatchProject.Models;

namespace SolarWatchProject.Services.ProcessData
{
    public interface IJsonProcessor
    {
        City GetProcessedGeoData(string geoData);
        SunRiseAndSetTime GetProcessedSunData(string sunData, City city, DateTime date);
    }
}
