namespace SolarWatchProjectBackend.Repositories
{
    public interface IGeoRepository
    {
        Task<string> GetLatLngByCity(string city);
    }
}
