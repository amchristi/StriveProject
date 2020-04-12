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
    public class PaymentAgent
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly UserAgent _userAgent;

        public PaymentAgent(HttpClient httpClient,
                       AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            
        }
        
        public async Task<bool> PayForCourses(CreditCard Card)
        {
                       
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authState.User.Claims.FirstOrDefault(m => m.Type == ClaimTypes.NameIdentifier).Value;

            // Check if customer is in stripe
            var requestCustomer = new HttpRequestMessage()
            {
                Method = new HttpMethod("GET"),                
                RequestUri = new Uri(("https://api.stripe.com/v1/customers/"+ userId))
            };
         
            requestCustomer.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "sk_test_LS4k44JWY54o4tWqb8uLLh5H00LuIW9et8");

            var CustomerResponse = await _httpClient.SendAsync(requestCustomer);            
            var CustomerResponseStatusCode = CustomerResponse.StatusCode;
            var CustomerResponseBody = await CustomerResponse.Content.ReadAsStringAsync();

            
            // Create new customer for stripe
            if (CustomerResponseStatusCode != System.Net.HttpStatusCode.OK) 
            {
                
                // Customer missing create customer
                var user = await _httpClient.GetJsonAsync<User>($"api/users/{userId}");
                var CreateCustomerRequest = new HttpRequestMessage()
                {
                    Method = new HttpMethod("POST"),
                    RequestUri = new Uri(("https://api.stripe.com/v1/customers"))
                };

                CreateCustomerRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "sk_test_LS4k44JWY54o4tWqb8uLLh5H00LuIW9et8");

                //Load message
                var CustomerKeyValues = new List<KeyValuePair<string, string>>();
                CustomerKeyValues.Add(new KeyValuePair<string, string>("id", userId));
                CustomerKeyValues.Add(new KeyValuePair<string, string>("email", user.Email));
                CustomerKeyValues.Add(new KeyValuePair<string, string>("name", user.FirstName + " " + user.LastName));


                // Encode and add the content
                CreateCustomerRequest.Content = new FormUrlEncodedContent(CustomerKeyValues);
                CreateCustomerRequest.Content.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue(
                    "application/x-www-form-urlencoded");


                var CreateCustomerResponse = await _httpClient.SendAsync(CreateCustomerRequest);
                var CreateCustomerResponseStatusCode = CreateCustomerResponse.StatusCode;
                var CreateCustomerResponseBody = await CreateCustomerResponse.Content.ReadAsStringAsync();

             /*   Console.WriteLine(CreateCustomerResponse);
                Console.WriteLine(CreateCustomerResponseStatusCode);
                Console.WriteLine(CreateCustomerResponseBody);*/
                
            }
 
            // Add the card
            var requestAddCard = new HttpRequestMessage() 
            { 
                Method = new HttpMethod("POST"),
                RequestUri = new Uri("https://api.stripe.com/v1/customers/" + userId + "/sources")               

            };

            requestAddCard.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "sk_test_LS4k44JWY54o4tWqb8uLLh5H00LuIW9et8");
            //Load message
            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("source[object]", "card"));
            keyValues.Add(new KeyValuePair<string, string>("source[number]", Card.CardNumber));
            keyValues.Add(new KeyValuePair<string, string>("source[exp_month]", Card.ExpMonth));
            keyValues.Add(new KeyValuePair<string, string>("source[exp_year]", Card.ExpYear));
            keyValues.Add(new KeyValuePair<string, string>("source[cvc]", Card.cvc));
            // Encode and add the content
            requestAddCard.Content = new FormUrlEncodedContent(keyValues);
            
            requestAddCard.Content.Headers.ContentType =
            new System.Net.Http.Headers.MediaTypeHeaderValue(
                "application/x-www-form-urlencoded");

            var response = await _httpClient.SendAsync(requestAddCard);
            var responseStatusCode = response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();
           
            //Console.WriteLine(response);
            //Console.WriteLine(responseStatusCode);
            Console.WriteLine(responseBody);

            // There should be a better way to pull out this value.
            int startOfId = responseBody.IndexOf("id");
            string cardID = responseBody.Substring(startOfId + 6, 29);

            Console.WriteLine(cardID);

            

           // Add the charge
           var requestAddCharge = new HttpRequestMessage()
           {
               Method = new HttpMethod("POST"),
               RequestUri = new Uri("https://api.stripe.com/v1/charges")

           };

            requestAddCharge.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "sk_test_LS4k44JWY54o4tWqb8uLLh5H00LuIW9et8");
            //Load message
            keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("amount", Card.amount.ToString()));
            keyValues.Add(new KeyValuePair<string, string>("currency", "usd"));
            keyValues.Add(new KeyValuePair<string, string>("source", cardID));
            keyValues.Add(new KeyValuePair<string, string>("description", "My first test charge"));
            keyValues.Add(new KeyValuePair<string, string>("customer", userId));
            // Encode and add the content
            requestAddCharge.Content = new FormUrlEncodedContent(keyValues);

            requestAddCharge.Content.Headers.ContentType =
            new System.Net.Http.Headers.MediaTypeHeaderValue(
                "application/x-www-form-urlencoded");

            response = await _httpClient.SendAsync(requestAddCharge);
            //Should have a the loop finish while the call is being made.
            /*for(int i = 0; i < 1000; i++)
            {
                for(int j = 0; j < 1000; j++)
                {
                    for(int k = 0; k < 1000; k++)
                    {

                    }
                }
            }*/
            responseStatusCode = response.StatusCode;
            responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(response);
            Console.WriteLine(responseStatusCode);
            Console.WriteLine(responseBody);

            return true;
        }
    }
}
