
using WEA_BE.DTO;

namespace WEA_BE.Services
{
    public interface IAuthService
    {
        Task<UserDto?> LoginAsync(string username, string password);
        Task<bool> RegisterAsync(string name, string username, string password);
    }
}