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
using MimeKit;
using System.IO;
using System.Text.RegularExpressions;

namespace StriveLearningSystem.Server.Controllers
{
    [ApiController]
    public class GradesController : Controller
    {
        public readonly GradeService _gradeservice;
        public readonly AnnouncementService _announcementService;
        public readonly CourseService _courseService;
       // public readonly FileService _fileService;

        public GradesController(GradeService gradeservice, AnnouncementService announcementService, CourseService courseService/*, FileService fileService*/)
        {
            _gradeservice = gradeservice;
            _announcementService = announcementService;
            _courseService = courseService;
            //_fileService = fileService;
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
        [Route("api/grades/{gradeId}/getgrades")]
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

        [Route("api/grades/{name}/grades")]
        [HttpGet]
        public IActionResult Download([FromRoute] String name)
        {
            try
            {

                var filePath = "\\AssignmentFiles\\"+name;
                //return File(filePath, MimeTypes.GetMimeType(filePath), Path.GetFileName(filePath));
                 return PhysicalFile(filePath, MimeTypes.GetMimeType(filePath), Path.GetFileName(filePath));
               // return Ok(_fileService.GetFile("1736yoda.jpg"));


            }
            catch (Exception e)
            {
                return BadRequest("Error Getting Grade");
            }
        }
    }
}

