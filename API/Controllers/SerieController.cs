using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.Controllers
{

    [ApiController]
    [Route("serie")]
    [Authorize]
    public class SerieController : ControllerBase
    {

        private readonly ISerieRepository _repository;

        public SerieController(ISerieRepository repository)
        {
            _repository = repository;
        }

        //[HttpGet]
        //public async Task<List<SerieModel>> Get()
        //{
        //    return  await _repository.GetAllSeries();
        //}

        [AllowAnonymous]
        [HttpGet()]
        public async Task<List<string>> GetSeriesTitles()
        {
            return await _repository.GetAllSeriesTitle();
        }

        [HttpGet("{id}")]
        public async Task<SerieModel> GetById(int id)
        {
            return await _repository.GetSerieById(id);
        }

        [AllowAnonymous]
        [HttpGet("favorites")]
        public async Task<List<string>> GetFavoriteSeries()
        {
            return await _repository.GetFavoriteSeries();
        }

        [HttpGet("title/{title}")]
        public async Task<SerieModel> GetByTitle(string title)
        {
            return await _repository.GetSerieInfoByTitle(title); ;
        }

        [AllowAnonymous]
        [HttpGet("{id}/episodes")]
        public async Task<SerieModel> GetSerieInfo(int id)
        {
            return await _repository.GetSerieInfoById(id);
        }

        [AllowAnonymous]
        [HttpGet("allseries")]
        public async Task<List<Series>> GetSeriesTitle()
        {
            return await _repository.GetSeriesTitle();
        }

        [AllowAnonymous]
        [HttpPost("filter")]
        public async Task<List<Series>> GetSeriesFilter([FromBody] Filter filter)
        {
            return await _repository.GetSeriesFilter(filter);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SerieModel serie)
        {
            return Ok(await _repository.AddSerie(serie));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<string> Put(SerieModel serie)
        {
            return await _repository.UpdateSerie(serie);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<string> Delete(int id)
        {
            return await _repository.DeleteSerie(id); ;
        }
    }
}
