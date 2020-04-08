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
    public class GradesController : Controller
    {
        public readonly GradeService _gradeservice;
        public readonly AnnouncementService _announcementService;
        public readonly CourseService _courseService;
        

        public GradesController(GradeService gradeservice, AnnouncementService announcementService, CourseService courseService)
        {
            _gradeservice = gradeservice;
            _announcementService = announcementService;
            _courseService = courseService;
        }


        [Route("api/grade/submitassignment")]
        [HttpPost]
        public async Task<IActionResult> SubmitAssignment([FromBody] Grade grade)
        {
            try
            {

                return Ok(await _gradeservice.SubmitAssignment(grade));

            }
            catch (Exception e)
            {
                return BadRequest("Error inserting Course");
            }
        }



        // Uploads the file to the server
        [Route("api/grade/fileAssignmentUpload")]
        [HttpPost]
        public IActionResult UploadAssignmentFile([FromBody] FileAssignment fileAssignment)
        {
            try
            {

                return Ok(_gradeservice.UploadAssignmentFile(fileAssignment));

            }
            catch (Exception e)
            {
                return BadRequest("Error uploading file");
            }
        }

        // Get grade by id
        [Route("api/grades/{gradeId}")]
        [HttpGet]
        public IActionResult GetGrade([FromRoute] int gradeId)
        {
            return Ok(_gradeservice.GetGrade(gradeId));
        }

        // Get grade by id
        [Route("api/grades/{gradeId}")]
        [HttpPut]
        public async Task<IActionResult> UpdateGrade([FromRoute] int gradeId, [FromBody] Grade grade)
        {
            var course = _courseService.GetCourseByAssignmentId(grade.AssignmentID);

            Announcement announcement = new Announcement
            {
                AnnouncementID = 0,
                Body = course.Title + " Your recently graded assignment: " + grade.Score,
                DateCreated = DateTime.Now,
                Title = "Grade Posted!",
                CourseID = course.CourseID
            };
            announcement = await _announcementService.CreateAnnouncement(announcement);
            return Ok(await _gradeservice.UpdateGrade(grade));
        }

        // Get grade off assignmentID and the userID
        [Route("api/grade/checkForGrade")]
        [HttpPost]
        public IActionResult CheckForGrade([FromBody] Grade grade)
        {
            try
            {
                return Ok(_gradeservice.CheckForGrade(grade));

            }
            catch (Exception e)
            {
                return BadRequest("Error Getting Grade");
            }
        }
    }
}

