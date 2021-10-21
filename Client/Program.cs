using BlazorShoppingApp.Client;
using BlazorShoppingApp.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

namespace BlazorShoppingApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<IWalletService, WalletService>();
            builder.Services.AddScoped<IShopService, ShopService>();
            builder.Services.AddBlazoredToast();

            builder.Services.AddScoped<AuthenticationStateProvider, ShopAuthProvider>();
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<IAuthService, AuthService>();

            await builder.Build().RunAsync(); 
        }
    }
}
