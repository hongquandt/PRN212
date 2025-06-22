using BusinessLogicLayer.Services;
using BusinessObjects;
using System;
using System.Windows;

namespace Pham_Hong_Quan_SE18D05_Assignment_01.Views
{
    public partial class CustomerEditWindow : Window
    {
        private readonly ICustomerService _customerService;
        private readonly Customer? _customer;

        public CustomerEditWindow(Customer? customer)
        {
            InitializeComponent();
            _customerService = new CustomerService();
            _customer = customer;

            if (_customer != null)
            {
                FullNameTextBox.Text = _customer.CustomerFullName;
                EmailTextBox.Text = _customer.EmailAddress;
                TelephoneTextBox.Text = _customer.Telephone;
                BirthdayDatePicker.SelectedDate = _customer.CustomerBirthday;
                PasswordTextBox.Text = _customer.Password;
                Title = "Edit Customer";
            }
            else
            {
                Title = "Add Customer";
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FullNameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                    string.IsNullOrWhiteSpace(PasswordTextBox.Text))
                {
                    ErrorTextBlock.Text = "Full Name, Email, and Password are required.";
                    ErrorTextBlock.Visibility = Visibility.Visible;
                    return;
                }

                var customer = new Customer
                {
                    CustomerID = _customer?.CustomerID ?? 0,
                    CustomerFullName = FullNameTextBox.Text,
                    EmailAddress = EmailTextBox.Text,
                    Telephone = TelephoneTextBox.Text,
                    CustomerBirthday = BirthdayDatePicker.SelectedDate ?? DateTime.Now,
                    Password = PasswordTextBox.Text,
                    CustomerStatus = 1
                };

                if (_customer == null)
                {
                    _customerService.AddCustomer(customer);
                }
                else
                {
                    _customerService.UpdateCustomer(customer);
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                ErrorTextBlock.Text = ex.Message;
                ErrorTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}