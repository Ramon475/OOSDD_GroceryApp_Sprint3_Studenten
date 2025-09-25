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
            Client? client = _clientService.Get(email);

            if (client == null)
            {

                var clients = _clientService.GetAll();

                var newClient = new Client(
                    id: clients.Count + 1,
                    name: name,
                    emailAddress: email,
                    password: password
                );

                _clientService.Add(newClient);

                return newClient;
            }
            
            return null;
        }
    }
}
