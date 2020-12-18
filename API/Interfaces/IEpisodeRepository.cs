using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IEpisodeRepository
    {

        Task<IEnumerable<EpisodeModel>> GetSerieEpisodes(string serieName);
        Task<IEnumerable<EpisodeModel>> GetSeriesSeasonEpisode(string name, string season);
        
        


    }
}
