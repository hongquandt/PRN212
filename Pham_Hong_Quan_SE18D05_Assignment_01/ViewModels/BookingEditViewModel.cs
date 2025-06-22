using BusinessLogicLayer.Services;
using BusinessObjects;
using DataAccessLayer.DataContext;
using DataAccessLayer.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Pham_Hong_Quan_SE18D05_Assignment_01.ViewModels
{
    public class BookingEditViewModel : BaseViewModel
    {
        private readonly ICustomerService _customerService;
        private readonly IRoomService _roomService;
        private readonly IBookingRepository _bookingRepository;
        private readonly BookingReservation _booking;
        private readonly InMemoryDataContext _context;

        private ObservableCollection<Customer> _customers;
        private ObservableCollection<RoomInformation> _rooms;
        private Customer? _selectedCustomer;
        private RoomInformation? _selectedRoom;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private string _errorMessage;
        private bool _isErrorVisible;
        private bool _isSaved;

        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set => SetProperty(ref _customers, value);
        }

        public ObservableCollection<RoomInformation> Rooms
        {
            get => _rooms;
            set => SetProperty(ref _rooms, value);
        }

        public Customer? SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                SetProperty(ref _selectedCustomer, value);
                OnPropertyChanged(nameof(CanSave));
            }
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

        public bool CanSave => SelectedCustomer != null && SelectedRoom != null && StartDate.HasValue && EndDate.HasValue;

        public bool IsSaved
        {
            get => _isSaved;
            set => SetProperty(ref _isSaved, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event EventHandler RequestClose;

        public BookingEditViewModel(BookingReservation booking)
        {
            _customerService = new CustomerService();
            _roomService = new RoomService();
            _bookingRepository = new BookingRepository();
            _booking = booking ?? throw new ArgumentNullException(nameof(booking));
            _context = InMemoryDataContext.Instance;

            Customers = new ObservableCollection<Customer>(_customerService.GetAllCustomers());
            Rooms = new ObservableCollection<RoomInformation>(_roomService.GetAllRooms());

            SaveCommand = new RelayCommand(Save, () => CanSave);
            CancelCommand = new RelayCommand(Cancel);

            InitializeData();
        }

        private void InitializeData()
        {
            if (_booking != null)
            {
                SelectedCustomer = Customers.FirstOrDefault(c => c.CustomerID == _booking.CustomerID);
                var bookingDetail = _context.BookingDetails.FirstOrDefault(bd => bd.BookingReservationID == _booking.BookingReservationID);
                if (bookingDetail != null)
                {
                    SelectedRoom = Rooms.FirstOrDefault(r => r.RoomID == bookingDetail.RoomID);
                    StartDate = bookingDetail.StartDate;
                    EndDate = bookingDetail.EndDate;
                }
                else
                {
                    // Tạo BookingDetail mặc định nếu chưa có
                    var defaultRoom = Rooms.FirstOrDefault();
                    if (defaultRoom != null)
                    {
                        SelectedRoom = defaultRoom;
                        StartDate = DateTime.Today;
                        EndDate = DateTime.Today.AddDays(1);
                    }
                }
            }
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

                // Calculate total price
                var days = (EndDate.Value - StartDate.Value).Days;
                if (days <= 0)
                {
                    ErrorMessage = "Booking duration must be at least 1 day.";
                    IsErrorVisible = true;
                    return;
                }

                var totalPrice = SelectedRoom.RoomPricePerDate * days;

                // Update BookingReservation
                _booking.BookingDate = DateTime.Now;
                _booking.TotalPrice = totalPrice;
                _booking.CustomerID = SelectedCustomer.CustomerID;
                _booking.Customer = SelectedCustomer;
                _bookingRepository.UpdateBooking(_booking);

                // Update or create BookingDetail
                var bookingDetail = _context.BookingDetails.FirstOrDefault(bd => bd.BookingReservationID == _booking.BookingReservationID);
                if (bookingDetail != null)
                {
                    bookingDetail.RoomID = SelectedRoom.RoomID;
                    bookingDetail.StartDate = StartDate.Value;
                    bookingDetail.EndDate = EndDate.Value;
                    bookingDetail.ActualPrice = totalPrice;
                    bookingDetail.RoomInformation = SelectedRoom;
                    bookingDetail.BookingReservation = _booking;
                }
                else
                {
                    bookingDetail = new BookingDetail
                    {
                        BookingReservationID = _booking.BookingReservationID,
                        RoomID = SelectedRoom.RoomID,
                        StartDate = StartDate.Value,
                        EndDate = EndDate.Value,
                        ActualPrice = totalPrice,
                        RoomInformation = SelectedRoom,
                        BookingReservation = _booking
                    };
                    _context.BookingDetails.Add(bookingDetail);
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