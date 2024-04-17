using SolarWatchProjectBackend.Models;

namespace SolarWatchProjectBackend.Services.ProcessData
{
    public interface IJsonProcessor
    {
        City GetProcessedGeoData(string geoData);
        SunRiseAndSetTime GetProcessedSunData(string sunData, City city, DateTime date);
    }
}
