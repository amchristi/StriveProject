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
    public class CoursesAgent
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public CoursesAgent(HttpClient httpClient,
                       AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<List<Course>> GetCoursesByUser()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authState.User.Claims.FirstOrDefault(m => m.Type == ClaimTypes.NameIdentifier).Value;
            var courses = await _httpClient.GetJsonAsync<List<Course>>($"api/users/{userId}/getStudentcourses");
            return courses;
        }

        public async Task<List<Course>> GetCoursesByTeacher()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authState.User.Claims.FirstOrDefault(m => m.Type == ClaimTypes.NameIdentifier).Value;
            var courses = await _httpClient.GetJsonAsync<List<Course>>($"api/users/{userId}/teacherCourses");
            return courses;
        }
        public async Task<List<Assignment>> GetAssignmentsByTeacher()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authState.User.Claims.FirstOrDefault(m => m.Type == ClaimTypes.NameIdentifier).Value;
            var assignments = await _httpClient.GetJsonAsync<List<Assignment>>($"api/users/{userId}/teacherAssignments");
            return assignments;
        }

        //Returns a sorted list of ungraded assignments by teacher.
        public async Task<List<Assignment>> GetAssignmentsUngradedByTeacher()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authState.User.Claims.FirstOrDefault(m => m.Type == ClaimTypes.NameIdentifier).Value;
            var assignments = await _httpClient.GetJsonAsync<List<Assignment>>($"api/users/{userId}/teacherUngradedAssignments");
            return assignments;
        }

        public async Task<List<Assignment>> GetAssignmentsByStudent()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authState.User.Claims.FirstOrDefault(m => m.Type == ClaimTypes.NameIdentifier).Value;
            var assignments = await _httpClient.GetJsonAsync<List<Assignment>>($"api/users/{userId}/studentAssignments");
            return assignments;
        }

        public async Task<List<Announcement>> GetAnnouncementsByStudent()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authState.User.Claims.FirstOrDefault(m => m.Type == ClaimTypes.NameIdentifier).Value;
            var announcements = await _httpClient.GetJsonAsync<List<Announcement>>($"api/users/{userId}/studentAnnouncements");
            return announcements;
        }

        //Returns the course object given a courseId
        public async Task<Course> GetCourseById(int courseId)
        {
            //Could check to see if user should have access to this course
            //var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            //var userId = authState.User.Claims.FirstOrDefault(m => m.Type == ClaimTypes.NameIdentifier).Value;
            var course = await _httpClient.GetJsonAsync<Course>($"api/courses/{courseId}/courseById");
            return course;
        }

        //Takes a new course and tries the enter it into the database. If correct it will return the course object with the ID otherwise null.
        public async Task<Course> AddNewCourse(Course newCourse)
        {
            try
            {
                var course = await _httpClient.PostJsonAsync<Course>("api/courses/addCourse", newCourse);
                return course;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //Updates a course and returns null if update failed.
        public async Task<Course> UpdateCourse(Course updatedCourse)
        {
            try
            {
                var course = await _httpClient.PostJsonAsync<Course>("api/courses/updateCourse", updatedCourse);
                return course;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        //Deletes course associated with the courseID passed in.
        public async Task<int> DeleteCourse(int deletedCourseID)
        {
            try
            {
                var courseID = await _httpClient.PostJsonAsync<int>("api/courses/deleteCourse", deletedCourseID);
                return courseID;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

    }
}
