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
    public class UserController : Controller
    {
        private IConfiguration _configuration;
        private readonly UserService _userService;

        public UserController(IConfiguration configuration, UserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [Route("api/users/{userId}")]
        [HttpGet]
        public IActionResult Profile([FromRoute] int userId)
        {
            try
            {
                return Ok( _userService.GetUser(userId));
            }
            catch (Exception e)
            {
                return BadRequest("Error can't get profile");
            }
        }

        [Route("api/users/{userId}")]
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] User user )
        {
            try
            {
                return Ok(await _userService.UpdateUser(user));
            }
            catch (Exception e)
            {
                return BadRequest("Error can't update profile");
            }
        }


        [Route("api/users")]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(_userService.GetAllUsers());
        }
    }
}
