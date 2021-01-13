using API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface ISerieRepository
    {

        Task<List<SerieModel>> GetAllSeries();
        Task<List<string>> GetAllSeriesTitle();
        Task<SerieModel> GetSerieById(int id);
        Task<List<string>> GetFavoriteSeries();
        Task<SerieModel> GetSerieInfoByTitle(string title);
        Task<SerieModel> GetSerieInfoById(int id);
        Task<List<Series>> GetSeriesTitle();
        Task<List<Series>> GetSeriesFilter(Filter filter);
        Task<string> AddSerie(SerieModel serie);
        Task<string> UpdateSerie(SerieModel serie);
        Task<string> DeleteSerie(int id);

    }
}
