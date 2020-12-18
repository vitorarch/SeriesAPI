using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("episode")]

    public class EpisodeController : ControllerBase
    {

        private readonly IEpisodeRepository _repository;

        public EpisodeController(IEpisodeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<List<EpisodeModel>> Get()
        {
            return null;
        }

        [HttpGet("{id}")]
        public async Task<EpisodeModel> GetById(int id)
        {
            return null;
        }

        [HttpGet("title/{title}")]
        public async Task<EpisodeModel> GetByTitle(string title)
        {
            return null;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<string> Post([FromBody] EpisodeModel serie)
        {
            return null;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<string> Put(int id)
        {
            return null;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<string> Delete(int id)
        {
            return null;
        }

    }
}
