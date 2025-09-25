using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.App.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly GlobalViewModel _global;

        // --- Login ---
        [ObservableProperty] private string email = "user3@mail.com";
        [ObservableProperty] private string password = "user3";

        // --- Feedback ---
        [ObservableProperty] private string errorMessage;

        // --- Register (met ObservableProperty, levert PascalCase properties op) ---
        [ObservableProperty] private string registerName = "test";
        [ObservableProperty] private string registerEmail = "test@mail.com";
        [ObservableProperty] private string registerPassword = "test";

        public LoginViewModel(IAuthService authService, GlobalViewModel global)
        {
            _authService = authService;
            _global = global;
        }

        [RelayCommand]
        private void Login()
        {
            Client? authenticatedClient = _authService.Login(Email, Password);
            if (authenticatedClient != null)
            {
                ErrorMessage = $"Welkom {authenticatedClient.Name}!";
                _global.Client = authenticatedClient;
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                ErrorMessage = "Ongeldige inloggegevens.";
            }
        }

        [RelayCommand]
        private void Register()
        {
            // Gebruik de gegenereerde properties (PascalCase), niet de private velden
            Client? createdClient = _authService.Register(RegisterName, RegisterEmail, RegisterPassword);
            if (createdClient != null)
            {
                ErrorMessage = $"Account aangemaakt. Welkom {createdClient.Name}!";
                _global.Client = createdClient;
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                ErrorMessage = "Registreren mislukt (email bestaat al of invoer ongeldig).";
            }
        }
    }
}
