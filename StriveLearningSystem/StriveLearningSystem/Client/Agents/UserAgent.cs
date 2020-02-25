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
    public class UserAgent
    {
        private readonly HttpClient _httpClient;

        public UserAgent(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User> GetUser(int userId)
        {
            try
            {
                var user = await _httpClient.GetJsonAsync<User>($"api/users/{userId}");
                return user;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<User> UpdateProfile(User updateUser)
        {
            try
            {
                var user = await _httpClient.PutJsonAsync<User>($"api/users/{updateUser.UserID}", updateUser);
                return user;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                var user = await _httpClient.GetJsonAsync<List<User>>($"api/users");
                return user;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
