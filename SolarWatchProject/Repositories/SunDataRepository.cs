using SolarWatchProject.Data;
using SolarWatchProject.Models;

namespace SolarWatchProject.Repositories
{
    public class SunDataRepository : ISunDataRepository
    {
        private readonly SolarWatchDbContext _dbContext;

        public SunDataRepository(SolarWatchDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<SunRiseAndSetTime> GetAllSunData()
        {
            return _dbContext.SunData;
        }
        public void AddCity(City city)
        {
            _dbContext.Cities.Add(city);
            _dbContext.SaveChanges();
        }

        public void AddSunData(SunRiseAndSetTime sunRiseAndSetTime)
        {
            _dbContext.SunData.Add(sunRiseAndSetTime);
            _dbContext.SaveChanges();
        }

        public City? GetCityByName(string cityName)
        {
            if (_dbContext.Cities.Any())
            {
                return _dbContext.Cities.FirstOrDefault(
                    c => c.Name.ToLower() == cityName.ToLower());
            }
            return null;
        }

        public SunRiseAndSetTime? UpdateSunData(SunRiseAndSetTime sunData)
        {
            var storedSunData = _dbContext.SunData.FirstOrDefault(
                c => c.Id == sunData.Id);
            if (storedSunData != null)
            {
                storedSunData.Description = sunData.Description;
                _dbContext.SaveChanges();
            }
            return storedSunData;
        }

        public bool DeleteSunData(SunRiseAndSetTime sunData)
        {
            try
            {
                _dbContext.SunData.Remove(_dbContext.SunData.SingleOrDefault(c => c.Id == sunData.Id));
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public SunRiseAndSetTime? GetSunData(string cityName, DateTime date)
        {
            if (_dbContext.Cities.Any())
            {
                return _dbContext.SunData.FirstOrDefault(
                s => s.City.ToLower() == cityName.ToLower()
                     && s.Date == date);
            }
            return null;
        }

        public SunRiseAndSetTime? GetSunData(int? id)
        {
            if (_dbContext.Cities.Any())
            {
                return _dbContext.SunData.FirstOrDefault(
                s => s.Id == id);
            }
            return null;
        }

        public SunRiseAndSetTime? GetSunDataById(int id)
        {
            return _dbContext.SunData.FirstOrDefault(
                s => s.Id == id);
        }
    }
}
