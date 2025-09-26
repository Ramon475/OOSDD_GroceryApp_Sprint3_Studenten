using Grocery.Core.Helpers;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IClientService _clientService;
        public AuthService(IClientService clientService)
        {
            _clientService = clientService;
        }
        public Client? Login(string email, string password)
        {
            Client? client = _clientService.Get(email);
            if (client == null) return null;
            if (PasswordHelper.VerifyPassword(password, client.Password)) return client;
            return null;
        }
        
        public Client? Register(string email, string password, string name)
        {
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(name))
                return null;

            var normalizedEmail = email.Trim().ToLowerInvariant();
            var trimmedName = name.Trim();
            
            if (_clientService.Get(normalizedEmail) is not null)
                return null;
            
            var hashed = PasswordHelper.HashPassword(password);
            
            var all = _clientService.GetAll();
            var nextId = all.Count == 0 ? 1 : all.Max(c => c.Id) + 1;

            var newClient = new Client(
                id: nextId,
                name: trimmedName,
                emailAddress: normalizedEmail,
                password: hashed
            );

            _clientService.Add(newClient);
            return newClient;
        }
    }
}
