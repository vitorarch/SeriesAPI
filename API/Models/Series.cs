using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Text;

namespace API.Models
{
    public class Series
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Seasons { get; set; }
        public string Situation { get; set; }
        public decimal Rating { get; set; }
        public int Awards { get; set; }

        public async Task<List<Series>> GetSeriesTitle()
        {
            using (var conn = new SQLiteConnection(Database.Database.Connect))
            {
                using (var cm = new SQLiteCommand(conn))
                {
                    List<Series> series = new List<Series>();
                    cm.CommandText = "SELECT Id, Title, Situation, Rating , Awards, (SELECT DISTINCT count(*) FROM Episode WHERE SerieId = Id Group By Episode) FROM Serie";
                    using(var reader = await cm.ExecuteReaderAsync())
                    {
                        string seasons = "(SELECT DISTINCT count(*) FROM Episode WHERE SerieId = Id Group By Episode)";
                        while (await reader.ReadAsync())
                        {
                            series.Add(new Series
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Seasons = reader.IsDBNull(reader.GetOrdinal(seasons))? 0: reader.GetInt32(reader.GetOrdinal(seasons)),
                                Situation = reader.GetString(reader.GetOrdinal("Situation")),
                                Rating = reader.GetDecimal(reader.GetOrdinal("Rating")),
                                Awards = reader.GetInt32(reader.GetOrdinal("Awards"))
                            });
                        }
                    return series;
                    }
                }
            }
        }


        public async Task<List<Series>> GetSeriesFilter(Filter filter)
        {
            string ratingFilter = ConvertingRating(filter.Rating);
            if (string.IsNullOrEmpty(filter.Title) && string.IsNullOrEmpty(filter.Situation1) && string.IsNullOrEmpty(filter.Situation2) && string.IsNullOrEmpty(ratingFilter))
            {
                return await GetSeriesTitle();
            }
            else
            {
                using (var conn = new SQLiteConnection(Database.Database.Connect))
                {
                    using (var cm = new SQLiteCommand(conn))
                    {
                        List<Series> series = new List<Series>();
                        cm.CommandText = FilterSeries(filter, ratingFilter);
                        using (var reader = await cm.ExecuteReaderAsync())
                        {
                            string seasons = "(SELECT DISTINCT count(*) FROM Episode WHERE SerieId = Id Group By Episode)";
                            while (await reader.ReadAsync())
                            {
                                series.Add(new Series
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Title = reader.GetString(reader.GetOrdinal("Title")),
                                    Seasons = reader.IsDBNull(reader.GetOrdinal(seasons)) ? 0 : reader.GetInt32(reader.GetOrdinal(seasons)),
                                    Situation = reader.GetString(reader.GetOrdinal("Situation")),
                                    Rating = reader.GetDecimal(reader.GetOrdinal("Rating")),
                                    Awards = reader.GetInt32(reader.GetOrdinal("Awards"))
                                });
                            }
                        }
                    return series;
                    }
                }
            }
        }

        private string FilterSeries(Filter Seriefilter, string ratingFilter)
        {
            StringBuilder filter = new StringBuilder();
            string baseQuery = "SELECT Id, Title, Situation, Rating , Awards, (SELECT DISTINCT count(*) FROM Episode WHERE SerieId = Id Group By Episode) FROM Serie WHERE";
            filter.Append(baseQuery);
            if (!string.IsNullOrEmpty(Seriefilter.Title))
                filter.Append($" Title LIKE '{Seriefilter.Title}%' AND");
            if (string.IsNullOrEmpty(Seriefilter.Situation1) ^ string.IsNullOrEmpty(Seriefilter.Situation2))
            {
                if (!string.IsNullOrEmpty(Seriefilter.Situation1))
                    filter.Append($" Situation = '{Seriefilter.Situation1}' AND");
                if (!string.IsNullOrEmpty(Seriefilter.Situation2))
                    filter.Append($" Situation = '{Seriefilter.Situation2}' AND");
            }
            if (!string.IsNullOrEmpty(ratingFilter))
                filter.Append($" Rating {ratingFilter}");

            string seriesFilter = filter.ToString();
            if (seriesFilter != string.Empty && seriesFilter.Substring(seriesFilter.Length - 3,3) == "AND")
                seriesFilter = seriesFilter.Remove(seriesFilter.Length - 3, 3);
            if (seriesFilter != string.Empty && seriesFilter.Substring(seriesFilter.Length - 5, 5) == "WHERE")
                seriesFilter = seriesFilter.Remove(seriesFilter.Length - 5, 5);

            seriesFilter.Append(';');
            return seriesFilter.ToString();
        }

        private string ConvertingRating(int? value)
        {
            string ratingFilter;
            switch (value)
            {
                case 1:
                    ratingFilter = "BETWEEN 0 AND 2 ";
                    return ratingFilter;
                case 2:
                    ratingFilter = "BETWEEN 2 AND 4 ";
                    return ratingFilter;
                case 3:
                    ratingFilter = "BETWEEN 4 AND 6 ";
                    return ratingFilter;
                case 4:
                    ratingFilter = "BETWEEN 6 AND 8 ";
                    return ratingFilter;
                case 5:
                    ratingFilter = "BETWEEN 8 AND 10 ";
                    return ratingFilter;
                default:
                    return ratingFilter = string.Empty;
            }
        }

    }
}
