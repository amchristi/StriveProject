using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace StriveLearningSystem.Client.Agents
{
    public class CoursesState
    {
        //private IReadOnlyList<Course> Courses { get; set; }
        public List<Course> Courses { get; private set; }
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public CoursesState(HttpClient httpClient,
                      AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        //Checks to see if Courses have been loaded and loads them only if needed
        public async Task LoadCourses()
        { 
            if (Courses == null)
            {               
                var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
                var userId = authState.User.Claims.FirstOrDefault(m => m.Type == ClaimTypes.NameIdentifier).Value;
                Courses = await _httpClient.GetJsonAsync<List<Course>>($"api/users/{userId}/getStudentcourses");                
            }
        }
    }
}
