
using Grocery.Core.Models;

namespace Grocery.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Client? Login(string email, string password);
        Client? Register(string email, string password, string name);
    }
}
