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
        private readonly UserService _userService;

        public IdentityController(IConfiguration configuration, UserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [Route("api/login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Credential cred)
        {

            try
            {
                var token = _userService.AuthenticateUser(cred.Password, cred.Email);

                var claims = new List<Claim>();

                claims.Add(new Claim(ClaimTypes.Name, token.FirstName + " " + token.LastName));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, token.UserID.ToString()));
                claims.Add(new Claim(ClaimTypes.Role, token.IsTeacher ? "Teacher" : "Student"));

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

                return BadRequest(new LoginResult { Successful = false, Error = ex.Message });
            }

        }
        [Route("api/register")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] User newUser)
        {
            try
            {
                return Ok(await _userService.AddNewUser(newUser));
            }
            catch (Exception e)
            {
                return BadRequest("Username already taken");
            }
        }




    }
}
