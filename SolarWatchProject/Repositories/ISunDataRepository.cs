using SolarWatchProject.Models;

namespace SolarWatchProject.Repositories
{
    public interface ISunDataRepository
    {
        public IEnumerable<SunRiseAndSetTime> GetAllSunData();
        public void AddCity(City city);
        public void AddSunData(SunRiseAndSetTime sunData);
        City? GetCityByName(string cityName);
        SunRiseAndSetTime? UpdateSunData(SunRiseAndSetTime sunData);
        bool DeleteSunData(SunRiseAndSetTime sunData);
        public SunRiseAndSetTime? GetSunData(string cityName, DateTime date);
        public SunRiseAndSetTime? GetSunData(int? id);
        public SunRiseAndSetTime? GetSunDataById(int id);
    }
}
