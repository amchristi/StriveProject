using Blazored.LocalStorage;
using Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using StriveLearningSystem.Client.Identity;
using StriveLearningSystem.Shared.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StriveLearningSystem.Client.Agents
{
    public class GradeAgent
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public GradeAgent(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<Grade> SubmitAssignment(Grade tempGrade)
        {
            try
            {
                var grade = await _httpClient.PostJsonAsync<Grade>("api/grade/submitassignment", tempGrade);
                return grade;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        // Uploads the assignment to the server
        public async Task<FileAssignment> UploadAssignmentFile(FileAssignment fileAssignment)
        {
            try
            {
                fileAssignment = await _httpClient.PostJsonAsync<FileAssignment>("api/grade/fileAssignmentUpload", fileAssignment);
                return fileAssignment;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Grade> GetGrade(int gradeId)
        {
            return await _httpClient.GetJsonAsync<Grade>($"api/grades/{gradeId}");
        }

        //Gets the grade from the assignmentID and the userId
        public async Task<Grade> CheckForGrade(Grade grade)
        {
            try
            {
                var outGrade = await _httpClient.PostJsonAsync<Grade>("api/grade/checkForGrade", grade);
                return outGrade;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Grade> GradeAssignment(Grade grade)
        {
            return await _httpClient.PutJsonAsync<Grade>($"api/grades/{grade.GradeID}", grade);

        }

    }
}
