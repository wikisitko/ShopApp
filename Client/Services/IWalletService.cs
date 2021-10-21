using System;
using System.Threading.Tasks;

namespace BlazorShoppingApp.Client.Services //Client???
{
    public interface IWalletService
    {
        int Money { get; }

        void Withdraw(int count);

        public event Action OnBalanceChanged;
        Task LoadMoney();
    }
}