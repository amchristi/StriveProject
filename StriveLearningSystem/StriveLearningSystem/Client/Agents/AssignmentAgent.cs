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
    public class AssignmentAgent
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AssignmentAgent(HttpClient httpClient,
                       AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<Assignment> AddNewAssignment(Assignment newAssignment)
        {
            try
            {
                var assignment = await _httpClient.PostJsonAsync<Assignment>("api/assignments/addAssignment", newAssignment);
                return assignment;
            }
            catch (Exception e)
            {
                return null;
            }
        }

       
        //Returns a list of all the assignments for the course
        public async Task<List<Assignment>> GetAssigmentByCourseID(int courseID)
        {
            try
            {
                var assignments = await _httpClient.GetJsonAsync<List<Assignment>>($"api/courses/{courseID}/assignments");
                return assignments;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Assignment> GetAssignment(int AssignmentID)
        {
            try
            {
                var assignment = await _httpClient.GetJsonAsync<Assignment>($"api/assignment/{AssignmentID}/getassignment");
                return assignment;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
   }
