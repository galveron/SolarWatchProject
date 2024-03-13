namespace SolarWatchProject.Services.Repositories
{
    public interface IGeoRepository
    {
        Task<string> GetLatLngByCity(string city);
    }
}
