using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class SeasonModel
    {
        public int Season { get; set; }
        public List<EpisodeModel> Episodes { get; set; }
    }
}
