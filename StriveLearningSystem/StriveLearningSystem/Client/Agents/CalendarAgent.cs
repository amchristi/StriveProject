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
            var events = await _httpClient.GetJsonAsync<List<CalendarEvent>>($"api/calendar/events");

            return events;
        }

    }
}
