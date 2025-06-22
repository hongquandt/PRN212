using BusinessObjects;
using BusinessLogicLayer.Services;
using DataAccessLayer.DataContext;
using DataAccessLayer.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Pham_Hong_Quan_SE18D05_Assignment_01.ViewModels
{
    public class BookingCreateViewModel : BaseViewModel
    {
        private readonly ICustomerService _customerService;
        private readonly IRoomService _roomService;
        private readonly IBookingRepository _bookingRepository;
        private readonly InMemoryDataContext _context;
        private readonly Customer _customer;
        private readonly RoomInformation _defaultRoom;

        private ObservableCollection<RoomInformation> _availableRooms;
        private RoomInformation? _selectedRoom;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private string _errorMessage;
        private bool _isErrorVisible;
        private bool _isSaved;

        public Customer Customer => _customer;
        public ObservableCollection<RoomInformation> AvailableRooms
        {
            get => _availableRooms;
            set => SetProperty(ref _availableRooms, value);
        }

        public RoomInformation? SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                SetProperty(ref _selectedRoom, value);
                OnPropertyChanged(nameof(CanSave));
            }
        }

        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                SetProperty(ref _startDate, value);
                OnPropertyChanged(nameof(CanSave));
            }
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                SetProperty(ref _endDate, value);
                OnPropertyChanged(nameof(CanSave));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public bool IsErrorVisible
        {
            get => _isErrorVisible;
            set => SetProperty(ref _isErrorVisible, value);
        }

        public bool CanSave => SelectedRoom != null && StartDate.HasValue && EndDate.HasValue;

        public bool IsSaved
        {
            get => _isSaved;
            set => SetProperty(ref _isSaved, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event EventHandler RequestClose;

        public BookingCreateViewModel(Customer customer, RoomInformation defaultRoom)
        {
            _customerService = new CustomerService();
            _roomService = new RoomService();
            _bookingRepository = new BookingRepository();
            _context = InMemoryDataContext.Instance;
            _customer = customer ?? throw new ArgumentNullException(nameof(customer));
            _defaultRoom = defaultRoom ?? throw new ArgumentNullException(nameof(defaultRoom));

            if (!_context.Customers.Contains(customer))
            {
                _context.Customers.Add(customer);
                System.Diagnostics.Debug.WriteLine($"[05:06 PM +07, 22/06/2025] Added Customer ID: {customer.CustomerID} to context");
            }
            if (!_context.Rooms.Contains(defaultRoom))
            {
                _context.Rooms.Add(defaultRoom);
                System.Diagnostics.Debug.WriteLine($"[05:06 PM +07, 22/06/2025] Added Room ID: {defaultRoom.RoomID} to context");
            }

            AvailableRooms = new ObservableCollection<RoomInformation>(_roomService.GetAllRooms().Where(r => r.RoomStatus == 1));
            SelectedRoom = _defaultRoom;
            StartDate = DateTime.Today;
            EndDate = DateTime.Today.AddDays(1);

            SaveCommand = new RelayCommand(Save, () => CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            try
            {
                if (StartDate >= EndDate)
                {
                    ErrorMessage = "End date must be after start date.";
                    IsErrorVisible = true;
                    return;
                }

                if (StartDate < DateTime.Today)
                {
                    ErrorMessage = "Start date cannot be in the past.";
                    IsErrorVisible = true;
                    return;
                }

                var days = (EndDate.Value - StartDate.Value).Days;
                if (days <= 0)
                {
                    ErrorMessage = "Booking duration must be at least 1 day.";
                    IsErrorVisible = true;
                    return;
                }

                var conflictingBookings = _context.BookingDetails
                    .Where(bd => bd.RoomID == SelectedRoom.RoomID &&
                                ((bd.StartDate <= EndDate && bd.EndDate >= StartDate) ||
                                 (bd.StartDate >= StartDate && bd.EndDate <= EndDate)));
                if (conflictingBookings.Any())
                {
                    ErrorMessage = "This room is already booked for the selected dates.";
                    IsErrorVisible = true;
                    return;
                }

                var totalPrice = SelectedRoom.RoomPricePerDate * days;

                var booking = new BookingReservation
                {
                    BookingDate = DateTime.Now,
                    TotalPrice = totalPrice,
                    CustomerID = _customer.CustomerID,
                    BookingStatus = 1,
                    Customer = _customer
                };
                _bookingRepository.AddBooking(booking);
                System.Diagnostics.Debug.WriteLine($"[05:06 PM +07, 22/06/2025] Added BookingReservation ID: {booking.BookingReservationID}");

                var bookingDetail = new BookingDetail
                {
                    BookingReservationID = booking.BookingReservationID,
                    RoomID = SelectedRoom.RoomID,
                    StartDate = StartDate.Value,
                    EndDate = EndDate.Value,
                    ActualPrice = totalPrice,
                    RoomInformation = SelectedRoom,
                    BookingReservation = booking
                };
                booking.BookingDetails.Add(bookingDetail);
                _context.BookingDetails.Add(bookingDetail);
                System.Diagnostics.Debug.WriteLine($"[05:06 PM +07, 22/06/2025] Added BookingDetail for BookingID: {booking.BookingReservationID}");

                var savedBooking = _bookingRepository.GetBookingById(booking.BookingReservationID);
                if (savedBooking == null)
                {
                    throw new Exception($"Failed to retrieve saved booking with ID {booking.BookingReservationID}");
                }
                System.Diagnostics.Debug.WriteLine($"[05:06 PM +07, 22/06/2025] Verified Booking ID: {savedBooking.BookingReservationID}, TotalPrice: {savedBooking.TotalPrice}");
                if (savedBooking.BookingDetails == null || !savedBooking.BookingDetails.Any())
                {
                    System.Diagnostics.Debug.WriteLine($"[05:06 PM +07, 22/06/2025] Warning: BookingDetails not loaded for BookingID {savedBooking.BookingReservationID}");
                }
                else
                {
                    var detail = savedBooking.BookingDetails.First();
                    System.Diagnostics.Debug.WriteLine($"[05:06 PM +07, 22/06/2025] Verified BookingDetail: StartDate: {detail.StartDate.ToString("dd/MM/yyyy")}, EndDate: {detail.EndDate.ToString("dd/MM/yyyy")}, RoomNumber: {detail.RoomInformation?.RoomNumber}");
                }

                var reloadedBookings = _bookingRepository.GetBookingsByCustomerId(_customer.CustomerID);
                System.Diagnostics.Debug.WriteLine($"[05:06 PM +07, 22/06/2025] Reloaded {reloadedBookings.Count} bookings for CustomerID {_customer.CustomerID}");
                foreach (var b in reloadedBookings)
                {
                    System.Diagnostics.Debug.WriteLine($"Reloaded BookingID: {b.BookingReservationID}, BookingDetails Count: {b.BookingDetails?.Count ?? 0}");
                }

                IsSaved = true;
                ErrorMessage = string.Empty;
                IsErrorVisible = false;
                RequestClose?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                IsErrorVisible = true;
                System.Diagnostics.Debug.WriteLine($"[05:06 PM +07, 22/06/2025] Save error: {ex}");
            }
        }

        private void Cancel()
        {
            IsSaved = false;
            ErrorMessage = string.Empty;
            IsErrorVisible = false;
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}