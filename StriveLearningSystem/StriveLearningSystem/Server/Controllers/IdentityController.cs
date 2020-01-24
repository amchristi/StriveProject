using StriveLearningSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using Data.Models;
using StriveLearningSystem.Shared.Models.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace StriveLearningSystem.Server.Controllers
{
    [ApiController]
    public class IdentityController : Controller
    {
        private IConfiguration _configuration;

        public IdentityController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("api/login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Credential cred)
        {

            try
            {
                //Call service function to see if user is authenticated.

                var claims = new List<Claim>();

                //claims.Add(new Claim(ClaimTypes.Name, token.FullName));
                //claims.Add(new Claim(ClaimTypes.NameIdentifier, token.UserId.ToString()));
                //Uncomment when service function is added.

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var jwtToken = new JwtSecurityToken(
                    _configuration["JwtIssuer"],
                    _configuration["JwtAudience"],
                    claims,
                    signingCredentials: creds);

                return Ok(new LoginResult { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(jwtToken) });
            }
            catch (Exception ex)
            {

                return BadRequest(new LoginResult { Successful = false, Error = "Username or password is invalid." });
            }

        }

    }
}
