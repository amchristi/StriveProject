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
        private readonly AnnouncementService _announcementService;
       

        public AssignmentsController(UserService userService, CourseService courseService, AssignmentService assignmentService, AnnouncementService announcementService )
        {
            _userService = userService;
            _courseService = courseService;
            _assignmentService = assignmentService;
            _announcementService = announcementService;
      
        }

        [Route("api/assignments/addAssignment")]
        [HttpPost]
        public async Task<IActionResult> AddAssignment([FromBody] Assignment newAssignment)
        {
            try
            {
                Announcement announcement = new Announcement
                {
                    AnnouncementID = 0,
                    Body = "New Assignment!",
                    CourseID = newAssignment.CourseID,
                    DateCreated = DateTime.Now,
                    Title = newAssignment.AssignmentTitle
                };
                announcement = await _announcementService.CreateAnnouncement(announcement);
                return Ok(await _assignmentService.AddNewAssignment(newAssignment));
            }
            catch (Exception e)
            {
                return BadRequest("Error inserting Course");
            }
        }

        //Returns a list of all the assignments by courseID
        [Route("api/courses/{courseID}/assignments")]
        [HttpGet]
        public IActionResult GetAssigmentByCourseID([FromRoute] int courseID)
        {
            return Ok(_assignmentService.GetAssigmentByCourseID(courseID));
        }

        //Return a single assignment from an assignmentID
        [Route("api/assignment/{AssignmentID}/getassignment")]
        [HttpGet]
        public IActionResult GetAssignmentByID([FromRoute] int AssignmentID)
        {
            return Ok(_assignmentService.GetAssignmentByAssignmentID(AssignmentID));
        }

      
        //Updates an assignment
        [Route("api/assignments/{AssignmentID}/update")]
        [HttpPut]
        public async Task<IActionResult> UpdateAssignment([FromRoute] int AssignmentID, [FromBody] Assignment assignment)
        {
            return Ok(await _assignmentService.UpdateAssignment(assignment));
        }

        //Gets all the users for a particular class and if they have a submission for that assignment.
        [Route("api/assignments/{assignmentId}/submissions")]
        [HttpGet]
        public IActionResult GetAssignmentSubmissions([FromRoute] int assignmentId)
        {
            return Ok(_assignmentService.GetAssignmentSubmissions(assignmentId));
        }

        [Route("api/assignments/{assignmentId}/grades")]
        [HttpGet]
        public IActionResult GetGradesByAssignment([FromRoute] int assignmentId)
        {
            return Ok(_assignmentService.GetGradesByAssignmentId(assignmentId));
        }

    }
}

