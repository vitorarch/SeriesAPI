
using API.Interfaces;

namespace API.Models
{
    public class EpisodeModel : IEpisode
    {
        public int Episode { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }

    }
}
