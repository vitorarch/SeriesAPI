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

        [HttpGet]
        public async Task<List<SerieModel>> Get()
        {
            return  await _repository.GetAllSeries();
        }

        [AllowAnonymousAttribute]
        [HttpGet("titles")]
        public async Task<List<string>> GetSeriesTitle()
        {
            return await _repository.GetAllSeriesTitle();
        }

        [HttpGet("{id}")]
        public async Task<SerieModel> GetById(int id)
        {
            return await _repository.GetSerieById(id);
        }

        [AllowAnonymous ]
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<string> Post([FromBody] SerieModel serie)
        {
            return await _repository.AddSerie(serie); ;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<string> Put(int id)
        {
            return await _repository.UpdateSerie(id); ;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<string> Delete(int id)
        {
            return await _repository.DeleteSerie(id); ;
        }
    }
}
