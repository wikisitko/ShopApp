using BlazorShoppingApp.Shared;
using System.Threading.Tasks;

namespace BlazorShoppingApp.Server.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<string>> Login(string email, string password);
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<bool> UserExists(string email);
    }
}