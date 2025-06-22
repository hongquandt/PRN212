using Pham_Hong_Quan_SE18D05_Assignment_01.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Pham_Hong_Quan_SE18D05_Assignment_01.Views
{
    public partial class LoginWindow : Window
    {
        private readonly LoginViewModel _viewModel;

        public LoginWindow()
        {
            InitializeComponent();
            _viewModel = new LoginViewModel();
            DataContext = _viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                _viewModel.Password = passwordBox.Password;
            }
        }
    }
}
