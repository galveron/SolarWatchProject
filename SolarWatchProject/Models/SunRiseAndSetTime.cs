namespace SolarWatchProjectBackend.Models
{
    public class SunRiseAndSetTime
    {
        public int Id { get; init; }
        public DateTime Date { get; init; }
        public string City { get; init; }
        public string SunRise { get; init; }
        public string SunSet { get; init; }
        public string? Description { get; set; }
    }
}
