using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{

    [ApiController]

    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly User _user;

        public TokenController(IConfiguration config)
        {
            _config = config;
            _user = new Models.User();
        }

        [AllowAnonymous]
        [HttpPost("singup")]
        public IActionResult SingUp([FromBody] User user)
        {
            var message = _user.RegisterUser(user);
            TokenManager.CreateToken(_config, user);
            return Ok(message);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            if(_user.ValidateUserCredentials(user.Email, user.Password, out User loggedUser))
            {
               return Ok(TokenManager.CreateToken(_config, loggedUser));
            }
            else return BadRequest("Usuário não encontrado");
        }

    }
}
