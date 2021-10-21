using BlazorShoppingApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorShoppingApp.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient httpClient;

        public AuthService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ServiceResponse<int>> Register(UserRegistration userRegistration)
        {
            var response = await httpClient.PostAsJsonAsync("/api/Auth/register", userRegistration);
            return await response.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }

        public async Task<ServiceResponse<string>> Login(UserLogin userLogin)
        {
            var response = await httpClient.PostAsJsonAsync("/api/Auth/login", userLogin);
            return await response.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}
