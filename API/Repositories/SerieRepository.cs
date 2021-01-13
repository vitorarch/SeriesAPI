using API.Models;
using Microsoft.AspNetCore.Mvc;
using API.Interfaces;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace API.Repositories
{
    public class SerieRepository : ISerieRepository
    {

        private readonly SerieModel _serie;
        private readonly Series _series;

        public SerieRepository()
        {
            _serie = new SerieModel();
            _series = new Series();
        }

        public async Task<List<SerieModel>> GetAllSeries()
        {
            return await _serie.GetSeries();
        }

        public async Task<List<string>> GetAllSeriesTitle()
        {
            return await _serie.GetSeriesTitles();
        }

        public async Task<SerieModel> GetSerieById(int id)
        {
            return await _serie.GetSerieById(id);
        }

        public async Task<List<string>> GetFavoriteSeries()
        {
            return await _serie.GetFavoriteSeries();
        }

        public async Task<SerieModel> GetSerieByTitle(string title)
        {
            return await _serie.GetSerieByTitle(title);
        }

        public async Task<SerieModel> GetSerieInfoById(int id)
        {
            return await  _serie.GetSerieInfoById(id);
        }

        public async Task<SerieModel> GetSerieInfoByTitle(string title)
        {
            return await _serie.GetSerieInfoByTitle(title);
        }

        public async Task<List<Series>> GetSeriesTitle()
        {
            return await _series.GetSeriesTitle();
        }

        public async Task<List<Series>> GetSeriesFilter(Filter filter)
        {
            return await _series.GetSeriesFilter(filter);
        }

        public async Task<string> AddSerie(SerieModel serie)
        {
            return await _serie.AddSerie(serie);
        }

        public async Task<string> UpdateSerie(SerieModel serie)
        {
            return await _serie.UpdateSerie(serie);
        }

        public async Task<string> DeleteSerie(int id)
        {
            return await _serie.DeleteSerie(id);
        }

    }
}
