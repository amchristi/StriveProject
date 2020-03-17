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

        public GradesController(GradeService gradeservice)
        {
            _gradeservice = gradeservice;
        }


        [Route("api/grade/submitassignment")]
        [HttpPost]
        public async Task<IActionResult> SubmitAssignment([FromBody] TempGrade grade)
        {
            try
            {
                if (!grade.IsFile)
                    return Ok(await _gradeservice.SubmitAssignmentText(grade));
                else
                    return Ok(await _gradeservice.SubmitAssignmentFile(grade));

            }
            catch (Exception e)
            {
                return BadRequest("Error inserting Course");
            }
        }

        // Uploads the file to the server
        [Route("api/grade/fileAssignmentUpload")]
        [HttpPost]
        public  IActionResult UploadAssignmentFile([FromBody] FileAssignment fileAssignment)
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
    }
}

