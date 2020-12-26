using API.Interfaces;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace API.Models
{
    public class EpisodeModel : IEpisode
    {
        public int Episode { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }

        //public async Task<bool> AddEpisodes(List<EpisodeModel> episodes)
        //{
        //    using (var conn = new SQLiteConnection(Database.Database.Connect))
        //    {
        //        using (var cm = new SQLiteCommand(conn))
        //        {
        //            {
        //                int episodeCounter = episodes.Count;
        //                string sqlQuery = "INSERT INTO Episodes VALUES";
        //                for (int i=1; i<=episodeCounter; i++)
        //                {
        //                    if (i == episodeCounter) sqlQuery += $"{episodes[i].SerieId}, {episodes[i].Season}, {episodes[i].Episode}, {episodes[i].Title}, {episodes[i].Duration};";
        //                    else sqlQuery += $" {episodes[i].SerieId}, {episodes[i].Season}, {episodes[i].Episode}, {episodes[i].Title}, {episodes[i].Duration},
        //                }

        //                cm.CommandText = sqlQuery;
        //                var affectedRows = await cm.ExecuteNonQueryAsync();
        //                if (affectedRows == episodeCounter) return true;
        //                else return false;
        //            }
        //        }
        //    }
        //}

        public async Task<bool> AddEpisodes(int serieId, List<SeasonModel> seasons)
        {
            using (var conn = new SQLiteConnection(Database.Database.Connect))
            {
                using (var cm = new SQLiteCommand(conn))
                {
                    string sqlQuery = "INSERT INTO Episode VALUES";
                    int episodeCounter = 0;

                    foreach(var season in seasons)
                    {
                        foreach(var episode in season.Episodes)
                        {
                            sqlQuery += $"('{ serieId }', '{ season.Season }', '{ episode.Episode }', '{ episode.Title }', '{ episode.Duration }'), ";
                            episodeCounter++;
                        }
                    }

                    sqlQuery = sqlQuery.Remove(sqlQuery.Length - 2,2);
                    sqlQuery += ";";
                    cm.CommandText = sqlQuery;

                    var affectedRows = await cm.ExecuteNonQueryAsync();
                    if (affectedRows == episodeCounter) return true;
                    else return false;
                }
            }
        }


        #region FETCH | ADD | UPDATE : AUXILIAR FUNCTIONS

        //private void GettingSerieId(SerieModel serie)
        //{
        //    SerieId = serie.Id;
        //}

        //private async Task SeparatingSeasons(List<SeasonModel> seasons)
        //{
        //    int seasonsCounter = seasons.Count;
        //    int counter = 1;
        //    while (counter != seasonsCounter)
        //    {
        //        await PassingEpisodesListBySeason(seasons[counter - 1]);
        //        counter++;
        //    }
        //}

        //private void PassingSerieToProperties(EpisodeModel episode)
        //{
        //    Episode = episode.Episode;
        //    Title = episode.Title;
        //    Duration = episode.Duration;
        //}

        //private void Fetch(DbDataReader reader)
        //{
        //    SerieId = reader.GetInt32(reader.GetOrdinal("SerieId"));
        //    Season = reader.GetInt32(reader.GetOrdinal("Season"));
        //    Episode = reader.GetInt32(reader.GetOrdinal("Episode"));
        //    Title = reader.GetString(reader.GetOrdinal("Title"));
        //    Duration = reader.GetString(reader.GetOrdinal("Duration"));
        //}

        //private void AddUpdateEpisode(SQLiteCommand cm)
        //{
        //    cm.Parameters.AddWithValue("@SerieId", SerieId);
        //    cm.Parameters.AddWithValue("@Season", Season);
        //    cm.Parameters.AddWithValue("@Episode", Episode);
        //    cm.Parameters.AddWithValue("@Title", Title);
        //    cm.Parameters.AddWithValue("@Duration", Duration);
        //}

        #endregion

    }
}
