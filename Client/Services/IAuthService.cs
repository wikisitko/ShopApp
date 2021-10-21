using BlazorShoppingApp.Shared;
using System.Threading.Tasks;

namespace BlazorShoppingApp.Client.Services
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegistration userRegistration);
        Task<ServiceResponse<string>> Login(UserLogin userLogin);
    }
}