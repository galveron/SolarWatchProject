namespace SolarWatchProjectBackend.Repositories
{
    public class SolarWatchRepository : ISolarWatchRepository
    {
        public async Task<string> GetSunRiseAndSet(DateTime date, double lat, double lng)
        {
            var url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lng}&date={date.ToShortTimeString()}";
            var client = new HttpClient();

            try
            {
                var result = await client.GetStringAsync(url);
                return result;
            }
            catch
            {
                throw new Exception("Error in SunApi");
            }

        }
    }
}
