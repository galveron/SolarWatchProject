namespace SolarWatchProject.Services.Repositories
{
    public class GeoRepository : IGeoRepository
    {
        private readonly IConfiguration _config;
        public GeoRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> GetLatLngByCity(string city)
        {
            var apiKey = _config["ApiKey"];
            var urlGeo = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&limit=5&appid={apiKey}";

            var clientGeo = new HttpClient();
            try
            {
                var response = await clientGeo.GetAsync(urlGeo);
                return response.Content.ReadAsStringAsync().Result;
            }
            catch
            {
                throw new Exception("Error in GetLngLng");
            }
        }
    }
}
