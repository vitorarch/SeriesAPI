using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Filter
    {
        public string Title { get; set; }
        public string Situation1 { get; set; }
        public string Situation2 { get; set; }
        public int? Rating { get; set; }
    }
}
