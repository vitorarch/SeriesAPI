using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Models
{
    public class TokenManager
    {

        public static string CreateToken(IConfiguration config, User user)
        {
            //Defino a chave de criptografia do token
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["SecurityKey"]));

            //Proprieades/ atributos do usuário
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };
            
            //Definir o algoritmo de criptografia para gerar o token
            var cred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Criando token
            var token = new JwtSecurityToken
            (
                issuer: "Vitor",
                audience: "Vitor",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: cred
            );

            var token2 = new JwtSecurityTokenHandler().WriteToken(token);
            
            return token2;
        }

    }
}
