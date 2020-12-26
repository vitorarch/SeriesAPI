using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SQLite;
using System.Threading.Tasks;
using API.Interfaces;

namespace API.Models
{
    public class SerieModel : ISerie
    {

        #region Properties

        public int? Id { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public string Year { get; set; }
        public decimal? Rating { get; set; }
        public string Producer { get; set; }
        public string Situation { get; set; }
        public int? Awards { get; set; }
        public List<SeasonModel> Seasons { get; set; }

        #endregion

        #region GET Method

        #region Get all Series
        public async Task<List<SerieModel>> GetSeries()
        {
            using (var conn = new SQLiteConnection(Database.Database.Connect))
            {
                using (var cm = new SQLiteCommand(conn))
                {
                    var series = new List<SerieModel>();
                    cm.CommandText = "SELECT * FROM Serie";
                    using (var reader = await cm.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            series.Add(new SerieModel
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Country = reader.GetString(reader.GetOrdinal("Country")),
                                Year = reader.GetString(reader.GetOrdinal("Year")),
                                Rating = reader.GetDecimal(reader.GetOrdinal("Rating")),
                                Producer = reader.GetString(reader.GetOrdinal("Producer")),
                                Situation = reader.GetString(reader.GetOrdinal("Situation")),
                                Awards = reader.GetInt32(reader.GetOrdinal("Awards"))
                            });
                        }
                    }
                return series;
                }
            }
        }

        public async Task<List<string>> GetSeriesTitles()
        {
            using (var conn = new SQLiteConnection(Database.Database.Connect))
            {
                using (var cm = new SQLiteCommand(conn))
                {
                    var series = new List<string>();
                    cm.CommandText = "SELECT [Title] FROM Serie";
                    using (var reader = await cm.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            series.Add(reader.GetString(reader.GetOrdinal("Title")));
                        }
                    }
                    return series;
                }
            }
        }

        #endregion

        #region Get Series by Id
        public async Task<SerieModel> GetSerieById(int id)
        {
            using (var conn = new SQLiteConnection(Database.Database.Connect))
            {
                using (var cm = new SQLiteCommand(conn))
                {
                    cm.CommandText = "SELECT * FROM Serie WHERE Id = @Id";
                    cm.Parameters.AddWithValue("@Id", id);
                    using (var reader = await cm.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Fetch(reader);
                        }
                        return this;
                    }
                }
            }
        }
        #endregion

        #region Get Series by Title

        public async Task<SerieModel> GetSerieByTitle(string title)
        {
            using (var conn = new SQLiteConnection(Database.Database.Connect))
            {
                using (var cm = new SQLiteCommand(conn))
                {
                    cm.CommandText = "SELECT * FROM Serie WHERE Title = @Title";
                    cm.Parameters.AddWithValue("@Title", title);
                    using (var reader = await cm.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Fetch(reader);
                        }
                        return this;
                    }
                }
            }
        }
        #endregion

        #region Get complete serie's information

        #region By Id
        public async Task<SerieModel> GetSerieInfoById(int serieId)
        {
            using (var conn = new SQLiteConnection(Database.Database.Connect))
            {
                using (var cm = new SQLiteCommand(conn))
                {
                    cm.CommandText = "SELECT * FROM Serie WHERE Id=@Id";
                    cm.Parameters.AddWithValue("@Id", serieId);
                    using (var reader = await cm.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            return new SerieModel
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Country = reader.GetString(reader.GetOrdinal("Country")),
                                Year = reader.GetString(reader.GetOrdinal("Year")),
                                Rating = reader.GetDecimal(reader.GetOrdinal("Rating")),
                                Producer = reader.GetString(reader.GetOrdinal("Producer")),
                                Situation = reader.GetString(reader.GetOrdinal("Situation")),
                                Awards = reader.GetInt32(reader.GetOrdinal("Awards")),
                                Seasons = GetSeason(serieId).Result
                            };
                        }
                        return null;
                    }
                }
            }
        }
        #endregion

        #region By Tilte

        public async Task<SerieModel> GetSerieInfoByTitle(string title)
        {
            using (var conn = new SQLiteConnection(Database.Database.Connect))
            {
                using (var cm = new SQLiteCommand(conn))
                {
                    cm.CommandText = "SELECT * FROM Serie WHERE Title=@Title";
                    cm.Parameters.AddWithValue("@Title", title);
                    using (var reader = await cm.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int serieId = reader.GetInt32(reader.GetOrdinal("Id"));
                            return new SerieModel
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Country = reader.GetString(reader.GetOrdinal("Country")),
                                Year = reader.GetString(reader.GetOrdinal("Year")),
                                Rating = reader.GetDecimal(reader.GetOrdinal("Rating")),
                                Producer = reader.GetString(reader.GetOrdinal("Producer")),
                                Situation = reader.GetString(reader.GetOrdinal("Situation")),
                                Awards = reader.GetInt32(reader.GetOrdinal("Awards")),
                                Seasons = GetSeason(serieId).Result
                            };
                        }
                        return null;
                    }
                }
            }
        }
        #endregion

        #endregion

        #endregion

        #region POST Method
        public async Task<string> AddSerie(SerieModel serie)
        {
            using (var conn = new SQLiteConnection(Database.Database.Connect))
            {
                using (var cm = new SQLiteCommand(conn))
                {
                    PassingSerieToProperties(serie);
                    //passar episodios
                    cm.CommandText = "INSERT INTO Serie VALUES (@Id, @Title, @Country, @Year, @Rating, @Producer, @Situation, @Awards)";
                    AddUpdateSerie(cm);
                    var serieAdded = await cm.ExecuteNonQueryAsync();
                    long serieId = conn.LastInsertRowId;

                    if(serie.Seasons == null)  // saving serie without season and episodes ( to debut )
                    {
                        if (serieAdded == 1 ) return $"Serie { Title } Added!";
                        else return $"Failed to save { Title }!";
                    }
                    else  // saving series with seasons and episodes
                    {
                        EpisodeModel episodes = new EpisodeModel();
                        bool episodesAdded = await episodes.AddEpisodes((int)serieId, Seasons);

                        if (serieAdded == 1 && episodesAdded) return $"Serie { Title } Added!";
                        else return $"Failed to save { Title }!";
                    }
                }
            }
        }

        #endregion

        #region PUT Method
        public async Task<string> UpdateSerie(int id)
        {
            using (var conn = new SQLiteConnection(Database.Database.Connect))
            {
                using (var cm = new SQLiteCommand(conn))
                {
                    cm.CommandText = "UPDATE Serie SET (Title = @Title, Country = @Country, Year = @Year, Rating = @Rating, Producer = @Producer,Situation =  @Situation, Awards = @Awards) WHERE Id = @Id";
                    cm.Parameters.AddWithValue("@Id", id);
                    AddUpdateSerie(cm);
                    var affectedRows = await cm.ExecuteNonQueryAsync();
                    if (affectedRows == 1) return $"Serie { Title } updated!";
                    else return $"Failed to update { Title }!";
                }
            }
        }

        #endregion

        #region DELETE Method
        public async Task<string> DeleteSerie(int id)
        {
            using (var conn = new SQLiteConnection(Database.Database.Connect))
            {
                using (var cm = new SQLiteCommand(conn))
                {
                    cm.CommandText = "SELECT * FROM Series WHERE Id = @Id";
                    cm.Parameters.AddWithValue("@Id", Id);
                    var affectedRows = await cm.ExecuteNonQueryAsync();
                    if (affectedRows == 1) return $"Serie deleted!";
                    else return $"Failed to delete serie!";
                }
            }
        }

        #endregion

        #region FETCH | ADD | UPDATE : AUXILIAR FUNCTIONS

        private void PassingSerieToProperties(SerieModel serie)
        {
            Id = null;
            Title = serie.Title;
            Country = serie.Country;
            Year = serie.Year;
            Rating = serie.Rating;
            Producer = serie.Producer;
            Situation = serie.Situation;
            Awards = serie.Awards;
            Seasons = serie.Seasons;
        }

        private void AddUpdateSerie(SQLiteCommand cm)
        {
            cm.Parameters.AddWithValue("@Id", null);
            cm.Parameters.AddWithValue("@Title", Title);
            cm.Parameters.AddWithValue("@Country", Country);
            cm.Parameters.AddWithValue("@Year", Year);
            cm.Parameters.AddWithValue("@Rating", Rating);
            cm.Parameters.AddWithValue("@Producer", Producer);
            cm.Parameters.AddWithValue("@Situation", Situation);
            cm.Parameters.AddWithValue("@Awards", Awards);
        }

        private void Fetch(DbDataReader reader)
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id"));
            Title = reader.GetString(reader.GetOrdinal("Title"));
            Country = reader.GetString(reader.GetOrdinal("Country"));
            Year = reader.GetString(reader.GetOrdinal("Year"));
            Rating = reader.GetDecimal(reader.GetOrdinal("Rating"));
            Producer = reader.GetString(reader.GetOrdinal("Producer"));
            Situation = reader.GetString(reader.GetOrdinal("Situation"));
            Awards = reader.GetInt32(reader.GetOrdinal("Awards"));
        }

        private async Task<List<SeasonModel>> GetSeason(int serieId)
        {
            using (var conn = new SQLiteConnection(Database.Database.Connect))
            {
                using (var cm = new SQLiteCommand(conn))
                {
                    var seasons = new List<SeasonModel>();
                    cm.CommandText = "SELECT DISTINCT Season FROM Episode WHERE SerieId=@SerieId";
                    cm.Parameters.AddWithValue("@SerieId", serieId);
                    using (var reader = await cm.ExecuteReaderAsync())
                    {
                        int season = 1;
                        while (await reader.ReadAsync())
                        {
                            seasons.Add(new SeasonModel
                            {
                                Season = season,
                                Episodes = await GetEpisodesBySeason(reader, serieId, season)
                            });
                            season++;
                        }
                    }
                    return seasons;
                }
            }
        }

        private async Task<List<EpisodeModel>> GetEpisodesBySeason(DbDataReader reader, int serieId, int season)
        {
            using (var conn = new SQLiteConnection(Database.Database.Connect))
            {
                using (var cm = new SQLiteCommand(conn))
                {
                    var episodes = new List<EpisodeModel>();
                    cm.CommandText = "SELECT * FROM Episode WHERE SerieId=@SerieId AND Season=@Season";
                    cm.Parameters.AddWithValue("@SerieId", serieId);
                    cm.Parameters.AddWithValue("@Season", season);

                    using (reader = await cm.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            episodes.Add(new EpisodeModel
                            {
                                Episode = reader.GetInt32(reader.GetOrdinal("Episode")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Duration = reader.GetString(reader.GetOrdinal("Duration"))
                            });
                        }
                        return episodes;
                    }
                }
            }
        }

        #endregion


    }
}