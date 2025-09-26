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

        // Login
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        [ObservableProperty] private string email = "user3@mail.com";
        
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        [ObservableProperty] private string password = "user3";
        
        [ObservableProperty] private string errorMessage = string.Empty;

        // Register
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string registerName = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string registerEmail = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string registerPassword = string.Empty;

        public LoginViewModel(IAuthService authService, GlobalViewModel global)
        {
            _authService = authService;
            _global = global;
        }

        // Enable button for login fields
        private bool CanLogin() =>
            !string.IsNullOrWhiteSpace(Email) &&
            !string.IsNullOrWhiteSpace(Password);

        // Enable button for register fields
        private bool CanRegister() =>
            !string.IsNullOrWhiteSpace(RegisterName) &&
            !string.IsNullOrWhiteSpace(RegisterEmail) &&
            !string.IsNullOrWhiteSpace(RegisterPassword);

        [RelayCommand(CanExecute = nameof(CanLogin))]
        private void Login()
        {
            var authenticatedClient = _authService.Login(Email.Trim(), Password);
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

        [RelayCommand(CanExecute = nameof(CanRegister))]
        private void Register()
        {
            var createdClient = _authService.Register(
                RegisterName.Trim(),
                RegisterEmail.Trim(),
                RegisterPassword
            );

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
