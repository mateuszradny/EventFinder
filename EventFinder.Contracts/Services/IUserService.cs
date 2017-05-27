using System.Threading.Tasks;

namespace EventFinder.Contracts.Services
{
    public interface IUserService
    {
        Task<User> CurrentUser { get; }

        Task<User> GetUser(string email);

        Task<User> Register(string email, string password);

        Task<bool> VerifyPassword(string email, string password);
    }
}