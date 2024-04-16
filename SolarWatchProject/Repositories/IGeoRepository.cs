namespace SolarWatchProject.Repositories
{
    public interface IGeoRepository
    {
        Task<string> GetLatLngByCity(string city);
    }
}
