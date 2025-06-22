using BusinessLogicLayer.Services;
using BusinessObjects;
using Pham_Hong_Quan_SE18D05_Assignment_01.Views;
using System.Windows;
using System.Windows.Input;

namespace Pham_Hong_Quan_SE18D05_Assignment_01.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;
        private readonly AuthenticationService _authService;

        public LoginViewModel()
        {
            _authService = new AuthenticationService(App.Configuration!);
            LoginCommand = new RelayCommand(Login);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoginCommand { get; }

        private void Login()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Please enter both email and password.";
                return;
            }

            var (isAuthenticated, role, customer) = _authService.Authenticate(Email, Password);

            if (isAuthenticated)
            {
                if (role == "Admin")
                {
                    var adminWindow = new AdminWindow();
                    adminWindow.Show();
                }
                else if (role == "Customer")
                {
                    var customerWindow = new CustomerWindow(customer!);
                    customerWindow.Show();
                }

                // Close login window
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is LoginWindow)
                    {
                        window.Close();
                        break;
                    }
                }
            }
            else
            {
                ErrorMessage = "Invalid email or password.";
            }
        }
    }
}
