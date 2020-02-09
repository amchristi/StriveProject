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
    public class CoursesController : Controller
    {
        private readonly UserService _userService;

        public CoursesController(UserService userService)
        {
            _userService = userService;
        }

        [Route("api/users/{userId}/courses")]
        [HttpGet]
        public IActionResult GetUsersCourses([FromRoute] int userId)
        {
            return Ok(_userService.GetClasses(userId));
        }

        //Takes in a teacher user id and returns the courses associated with that teacher.
        [Route("api/users/{userId}/teacherCourses")]
        [HttpGet]
        public IActionResult GetTeacherCourses([FromRoute] int userId)
        {
            return Ok(_userService.GetCourseTaughtByTeacher(userId));
        }

        [Route("api/users/{userId}/studentAssignments")]
        [HttpGet]
        public IActionResult GetStudentAssignmets([FromRoute] int userId) 
        {
            return Ok(_userService.GetStudentAssignmentsByUserId(userId));
        }

        [Route("api/users/{userId}/teacherAssignments")]
        [HttpGet]
        public IActionResult GetTeacherAssignmets([FromRoute] int userId)
        {
            return Ok(_userService.GetTeacherAssignmentsByUserId(userId));
        }
    }
}

