using BusinessLogicLayer.Services;
using BusinessObjects;
using DataAccessLayer.Repositories;
using System.Windows;
using System.Windows.Controls;

namespace Pham_Hong_Quan_SE18D05_Assignment_01.Views
{
    public partial class AdminWindow : Window
    {
        private readonly ICustomerService _customerService;
        private readonly IRoomService _roomService;
        private readonly IBookingRepository _bookingRepository;

        public AdminWindow()
        {
            InitializeComponent();
            _customerService = new CustomerService();
            _roomService = new RoomService();
            _bookingRepository = new BookingRepository();
            LoadCustomers();
            LoadRooms();
            LoadBookings();
        }

        private void LoadCustomers()
        {
            CustomersDataGrid.ItemsSource = _customerService.GetAllCustomers();
        }

        private void LoadRooms()
        {
            RoomsDataGrid.ItemsSource = _roomService.GetAllRooms();
        }

        private void LoadBookings()
        {
            BookingsDataGrid.ItemsSource = _bookingRepository.GetAllBookings();
        }

        private void EditCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int customerId)
            {
                var customer = _customerService.GetCustomerById(customerId);
                if (customer != null)
                {
                    var editWindow = new CustomerEditWindow(customer);
                    if (editWindow.ShowDialog() == true)
                    {
                        LoadCustomers();
                    }
                }
            }
        }

        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int customerId)
            {
                if (MessageBox.Show("Are you sure you want to delete this customer?", "Confirm Delete",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _customerService.DeleteCustomer(customerId);
                    LoadCustomers();
                }
            }
        }

        private void EditRoom_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int roomId)
            {
                var room = _roomService.GetRoomById(roomId);
                if (room != null)
                {
                    var editWindow = new RoomEditWindow(room);
                    if (editWindow.ShowDialog() == true)
                    {
                        LoadRooms();
                    }
                }
            }
        }

        private void DeleteRoom_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int roomId)
            {
                if (MessageBox.Show("Are you sure you want to delete this room?", "Confirm Delete",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _roomService.DeleteRoom(roomId);
                    LoadRooms();
                }
            }
        }

        private void CreateBooking_Click(object sender, RoutedEventArgs e)
        {
            var bookingWindow = new BookingCreateWindow(null);
            if (bookingWindow.ShowDialog() == true)
            {
                LoadBookings();
            }
        }

        private void EditBooking_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int bookingId)
            {
                var booking = _bookingRepository.GetBookingById(bookingId);
                if (booking != null)
                {
                    var viewModel = new ViewModels.BookingEditViewModel(booking);
                    var editWindow = new BookingEditWindow(viewModel);
                    if (editWindow.ShowDialog() == true)
                    {
                        LoadBookings();
                    }
                }
            }
        }

        private void DeleteBooking_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int bookingId)
            {
                if (MessageBox.Show("Are you sure you want to delete this booking?", "Confirm Delete",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _bookingRepository.DeleteBooking(bookingId);
                    LoadBookings();
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