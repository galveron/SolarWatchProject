namespace SolarWatchProject.Services.Repositories
{
    public interface ISolarWatchRepository
    {
        Task<string> GetSunRiseAndSet(DateTime date, double lat, double lng);
    }
}
