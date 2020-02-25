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
using Microsoft.AspNetCore.Authorization;

namespace StriveLearningSystem.Server.Controllers
{
    [ApiController]
    public class AssignmentsController : Controller
    {
        private readonly AssignmentService _assignmentService;
        private readonly UserService _userService;
        private readonly CourseService _courseService;

        public AssignmentsController(UserService userService, CourseService courseService, AssignmentService assignmentService)
        {
            _userService = userService;
            _courseService = courseService;
            _assignmentService = assignmentService;
        }

        [Route("api/assignments/addAssignment")]
        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] Assignment newAssignment)
        {
            try
            {
                return Ok(await _assignmentService.AddNewAssignment(newAssignment));
            }
            catch (Exception e)
            {
                return BadRequest("Error inserting Course");
            }
        }
    }
}

