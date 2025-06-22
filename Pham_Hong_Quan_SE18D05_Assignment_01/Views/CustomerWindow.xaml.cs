using BusinessLogicLayer.Services;
using BusinessObjects;
using DataAccessLayer.Repositories;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Pham_Hong_Quan_SE18D05_Assignment_01.Views
{
    public partial class CustomerWindow : Window
    {
        private readonly Customer _customer;
        private readonly ICustomerService _customerService;
        private readonly IRoomService _roomService;
        private readonly IBookingRepository _bookingRepository;

        public CustomerWindow(Customer customer)
        {
            InitializeComponent();
            _customer = customer;
            _customerService = new CustomerService();
            _roomService = new RoomService();
            _bookingRepository = new BookingRepository();
            WelcomeText.Text = $"Welcome, {customer.CustomerFullName}!";
            LoadProfile();
            LoadRooms();
            LoadBookingHistory();
        }

        private void LoadProfile()
        {
            var customer = _customerService.GetCustomerById(_customer.CustomerID);
            if (customer != null)
            {
                CustomerIdTextBlock.Text = customer.CustomerID.ToString();
                FullNameTextBlock.Text = customer.CustomerFullName;
                EmailTextBlock.Text = customer.EmailAddress;
                TelephoneTextBlock.Text = customer.Telephone;
                BirthdayTextBlock.Text = customer.CustomerBirthday.ToString("dd/MM/yyyy");
            }
        }

        private void LoadRooms()
        {
            RoomsDataGrid.ItemsSource = _roomService.GetAllRooms();
        }

        private void LoadBookingHistory()
        {
            var bookings = _bookingRepository.GetBookingsByCustomerId(_customer.CustomerID);
            BookingsDataGrid.ItemsSource = bookings;
            System.Diagnostics.Debug.WriteLine($"[05:06 PM +07, 22/06/2025] Loaded {bookings.Count} bookings for CustomerID {_customer.CustomerID}");
            foreach (var booking in bookings)
            {
                System.Diagnostics.Debug.WriteLine($"BookingID: {booking.BookingReservationID}, TotalPrice: {booking.TotalPrice}");
                if (booking.BookingDetails == null)
                {
                    System.Diagnostics.Debug.WriteLine($"[05:06 PM +07, 22/06/2025] Warning: BookingID {booking.BookingReservationID} has null BookingDetails");
                }
                else if (booking.BookingDetails.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine($"[05:06 PM +07, 22/06/2025] Warning: BookingID {booking.BookingReservationID} has empty BookingDetails");
                }
                else
                {
                    var detail = booking.BookingDetails[0];
                    System.Diagnostics.Debug.WriteLine($"BookingID: {booking.BookingReservationID}, StartDate: {detail.StartDate.ToString("dd/MM/yyyy")}, EndDate: {detail.EndDate.ToString("dd/MM/yyyy")}, RoomNumber: {detail.RoomInformation?.RoomNumber}");
                }
            }
        }

        private void EditProfile_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new CustomerEditWindow(_customer);
            if (editWindow.ShowDialog() == true)
            {
                LoadProfile();
                WelcomeText.Text = $"Welcome, {_customerService.GetCustomerById(_customer.CustomerID).CustomerFullName}!";
            }
        }

        private void BookRoom_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int roomId)
            {
                var room = _roomService.GetRoomById(roomId);
                if (room != null)
                {
                    var viewModel = new ViewModels.BookingCreateViewModel(_customer, room);
                    var bookingWindow = new BookingCreateWindow(viewModel);
                    if (bookingWindow.ShowDialog() == true)
                    {
                        System.Diagnostics.Debug.WriteLine($"[05:06 PM +07, 22/06/2025] Booking attempt succeeded for CustomerID {_customer.CustomerID}");
                        LoadRooms();
                        LoadBookingHistory();
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[05:06 PM +07, 22/06/2025] Booking attempt failed or cancelled for CustomerID {_customer.CustomerID}");
                    }
                }
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }
    }
}