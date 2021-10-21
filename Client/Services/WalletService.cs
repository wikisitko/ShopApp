using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorShoppingApp.Client.Services //Client???
{
    public class WalletService : IWalletService
    {
        private int money = 0;
        private readonly HttpClient http;

        public event Action OnBalanceChanged;

        public int Money { get => money; }

        public WalletService(HttpClient http)
        {
            this.http = http;
        }

        public void Withdraw(int count)
        {
            money -= count;
            OnBalanceChanged?.Invoke(); //? - jesli nie ma podpietego zadnego "sluchacza"

            //Action func = () => Console.WriteLine("Hello world!");
            //func();
            //func.Invoke();

            ////wersja 1 wywolania z zabezpieczeniem
            //if(func != null)
            //{
            //    func();
            //}

            ////wersja 2 -||-
            //func?.Invoke();
        }

        public async Task LoadMoney()
        {
            //var response = await http.GetAsync("/api/Shop/money");
            //if(response.StatusCode == System.Net.HttpStatusCode.OK)
            //{

            //    OnBalanceChanged?.Invoke();
            //}
            try
            {
                money = await http.GetFromJsonAsync<int>("api/Shop/money");
            }
            catch(Exception)
            {
                Console.WriteLine("Blad ladowania pieniedzy!");
                money = 5000;
            }
            OnBalanceChanged?.Invoke();
        }
    }
}
