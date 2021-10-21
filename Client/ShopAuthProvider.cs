using Blazored.LocalStorage;
using BlazorShoppingApp.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorShoppingApp.Client
{
    public class ShopAuthProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localStorage;
        private readonly HttpClient httpClient;
        private readonly IShopService shopService;
        private readonly IWalletService wallet;

        public ShopAuthProvider(ILocalStorageService localStorage, HttpClient httpClient, IShopService shopService, IWalletService wallet)
        {
            this.localStorage = localStorage;
            this.httpClient = httpClient;
            this.shopService = shopService;
            this.wallet = wallet;
        }

        //ta metoda stwierdza czy ma zalogowac uzytkownika czy nie
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await localStorage.GetItemAsStringAsync("token");
            var identity = new ClaimsIdentity();
            httpClient.DefaultRequestHeaders.Authorization = null;

            if(!string.IsNullOrEmpty(token))
            {
                try
                {
                    identity = new ClaimsIdentity(ParseClaimsFormJwt(token), "jwt");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    
                }
               catch(Exception)
                {
                    await localStorage.RemoveItemAsync("token");
                    identity = new ClaimsIdentity();
                }
            }

            var state = new AuthenticationState(new ClaimsPrincipal(identity));
            NotifyAuthenticationStateChanged(Task.FromResult(state));

            Console.WriteLine("OK: " + token);

            return state;
        }

        private byte[] Parse64BaseWithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }
            return Convert.FromBase64String(base64);
        }
        private IEnumerable<Claim> ParseClaimsFormJwt(string jwt)
        {
            var payload = jwt.Split(".")[1];
            var jsonBytes = Parse64BaseWithoutPadding(payload); 
            var keyValues = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            //var claims = new List<Claim>();
            //foreach (var item in keyValues)
            //{
            //    claims.Add(new Claim(item.Key, item.Value.ToString()));
            //}
            //return claims;
            foreach (var item in keyValues)
            {
                Console.WriteLine(item);
            }
            return keyValues.Select(x => new Claim(x.Key, x.Value.ToString()));
        }
    }
}
