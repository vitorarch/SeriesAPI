using API.Models;
using System.Collections.Generic;

namespace API.Interfaces
{
    public interface ISerie
    {
        int? Id { get; set; }
        string Title { get; set; }
        string Country { get; set; }
        string Year { get; set; }
        decimal? Rating { get; set; }
        string Producer { get; set; }
        string Situation { get; set; }
        int? Awards { get; set; }
        List<SeasonModel> Seasons { get; set; }
    }
}
