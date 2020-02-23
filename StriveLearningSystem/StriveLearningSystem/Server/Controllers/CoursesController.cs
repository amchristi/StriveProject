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
    public class CoursesController : Controller
    {
        private readonly AssignmentService _assignmentService;
        private readonly UserService _userService;
        private readonly CourseService _courseService;

        public CoursesController(UserService userService, CourseService courseService, AssignmentService assignmentService)
        {
            _userService = userService;
            _courseService = courseService;
            _assignmentService = assignmentService;
        }

        [Route("api/users/{userId}/getStudentcourses")]
        [HttpGet]
        public IActionResult GetUsersCourses([FromRoute] int userId)
        {
            return Ok(_courseService.GetCoursesByStudentID(userId));
        }

        //Takes in a teacher user id and returns the courses associated with that teacher.
        [Route("api/users/{userId}/teacherCourses")]
        [HttpGet]
        public IActionResult GetTeacherCourses([FromRoute] int userId)
        {
            return Ok(_courseService.GetCourseTaughtByTeacher(userId));
        }

        [Route("api/users/{userId}/studentAssignments")]
        [HttpGet]
        public IActionResult GetStudentAssignmets([FromRoute] int userId)
        {
            return Ok(_assignmentService.GetStudentAssignmentsByUserId(userId));
        }

        [Route("api/users/{userId}/studentAnnouncements")]
        [HttpGet]
        public IActionResult GetStudentAnnouncements([FromRoute] int userId)
        {
            return Ok(_userService.GetStudentAnnnouncementsByUserId(userId));
        }

        [Route("api/users/{userId}/teacherAssignments")]
        [HttpGet]
        public IActionResult GetTeacherAssignmets([FromRoute] int userId)
        {
            return Ok(_assignmentService.GetTeacherAssignmentsByUserId(userId));
        }

        [Authorize]
        [Route("api/calendar/events")]
        [HttpGet]
        public IActionResult GetCalendarItems()
        {
            int userId;
            int.TryParse(HttpContext.User.Claims.FirstOrDefault(m => m.Type == ClaimTypes.NameIdentifier).Value, out userId);
            var role = HttpContext.User.Claims.FirstOrDefault(m => m.Type == ClaimTypes.Role).Value;
            return Ok(_assignmentService.GetCalendarItems(userId, role));
        }

        [Route("api/users/{userId}/teacherUngradedAssignments")]
        [HttpGet]
        public IActionResult GetTeacherUngradedAssigments([FromRoute] int userId)
        {
            return Ok(_assignmentService.GetTeacherUngradedAssignmentsByUserId(userId));
        }

        //Takes in a courseId and returns a course object
        [Route("api/courses/{courseId}/courseById")]
        [HttpGet]
        public IActionResult GetCourse([FromRoute] int courseId)
        {
            return Ok(_courseService.GetCourseById(courseId));
        }

        [Route("api/courses/addCourse")]
        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] Course newCourse)
        {
            int userId;
            int.TryParse(HttpContext.User.Claims.FirstOrDefault(m => m.Type == ClaimTypes.NameIdentifier).Value, out userId);
            newCourse.TeacherID = userId;
            try
            {
                return Ok(await _courseService.AddNewCourse(newCourse));
            }
            catch (Exception e)
            {
                return BadRequest("Error inserting Course");
            }
        }

        //Updates a course in the database will throw an error if the courseID does not exist.
        [Route("api/courses/updateCourse")]
        [HttpPost]
        public async Task<IActionResult> UpdateCourse([FromBody] Course updatedCourse)
        {
            try
            {
                return Ok(await _courseService.UpdateCourse(updatedCourse));
            }
            catch (Exception e)
            {
                return BadRequest("Error updating Course");
            }
        }

        //Sends request to delete course based on Course ID
        [Route("api/courses/deleteCourse")]
        [HttpPost]
        public async Task<IActionResult> DeleteCourse([FromBody] int deletedCourseID)
        {
            try
            {
                return Ok(await _courseService.DeleteCourse(deletedCourseID));
            }
            catch (Exception e)
            {
                return BadRequest("Error deleting Course");
            }
        }

        [Route("api/courses/getCourses")]
        [HttpGet]
        public IActionResult getCourses()
        {
            try
            {
                return Ok(_courseService.getCourses());
            }
            catch
            {
                return BadRequest("Error retriving courses");
            }
        }

        [Route("api/courses/registerStudentForCourse")]
        [HttpPost]
        public async Task<IActionResult> registerStudentForCourse([FromBody] UserCourse UserCourseToRegister)
        {
            try
            {
                return Ok(await _courseService.registerStudentForCourse(UserCourseToRegister));
            }
            catch (Exception e)
            {
                return BadRequest("Error registering course");
            }
        }

        [Route("api/courses/dropStudentCourseRegistration")]
        [HttpPost]
        public async Task<IActionResult> dropStudentCourseRegistration([FromBody] UserCourse UserCourseToDrop)
        {
            try
            {
                return Ok(await _courseService.dropStudentCourseRegistration(UserCourseToDrop));
            }
            catch (Exception e)
            {
                return BadRequest("Error dropping course");
            }
        }

        
    }
}

