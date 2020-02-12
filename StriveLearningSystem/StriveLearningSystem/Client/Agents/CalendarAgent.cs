using Blazored.LocalStorage;
using Data.DTOs;
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
    public class CalendarAgent
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public CalendarAgent(HttpClient httpClient,
                       AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<List<CalendarEvent>> GetCalendarEvents()
        {
            List<CalendarEvent> events = new List<CalendarEvent>();
            events.Add(new CalendarEvent() { AllDay = true, Start = DateTime.UtcNow, Title = "Test Event 1" });
            events.Add(new CalendarEvent() { Start = DateTime.UtcNow, End = DateTime.UtcNow.AddHours(5.5), Title = "Test Event 2" });
            events.Add(new CalendarEvent() { Start = DateTime.UtcNow.AddDays(-2), End = DateTime.UtcNow.AddDays(-1.5), Title = "Test Event 3" });
            events.Add(new CalendarEvent() { Start = DateTime.UtcNow.AddDays(-1), End = DateTime.UtcNow.AddDays(-1), Title = "Test Event 4" });
            //var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            //var userId = authState.User.Claims.FirstOrDefault(m => m.Type == ClaimTypes.NameIdentifier).Value;
            //var courses = await _httpClient.GetJsonAsync<List<Course>>($"api/users/{userId}/courses");
            return events;
        }

    }
}
