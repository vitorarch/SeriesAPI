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
        Task<SerieModel> GetSerieInfoByTitle(string title);
        Task<SerieModel> GetSerieInfoById(int id);
        Task<string> AddSerie(SerieModel serie);
        Task<string> UpdateSerie(int id);
        Task<string> DeleteSerie(int id);

    }
}
