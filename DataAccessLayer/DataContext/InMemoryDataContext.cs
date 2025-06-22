using BusinessObjects;

namespace DataAccessLayer.DataContext
{
    public class InMemoryDataContext
    {
        private static InMemoryDataContext? _instance;
        private static readonly object _lock = new object();

        public List<Customer> Customers { get; private set; }
        public List<RoomType> RoomTypes { get; private set; }
        public List<RoomInformation> Rooms { get; private set; }
        public List<BookingReservation> BookingReservations { get; private set; }
        public List<BookingDetail> BookingDetails { get; private set; }

        private InMemoryDataContext()
        {
            InitializeData();
        }

        public static InMemoryDataContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new InMemoryDataContext();
                    }
                }
                return _instance;
            }
        }

        private void InitializeData()
        {
            RoomTypes = new List<RoomType>
            {
                new RoomType { RoomTypeID = 1, RoomTypeName = "Standard", TypeDescription = "Standard room with basic amenities", TypeNote = "Budget-friendly option" },
                new RoomType { RoomTypeID = 2, RoomTypeName = "Deluxe", TypeDescription = "Deluxe room with premium amenities", TypeNote = "Mid-range option" },
                new RoomType { RoomTypeID = 3, RoomTypeName = "Suite", TypeDescription = "Luxury suite with all amenities", TypeNote = "Premium option" }
            };

            Rooms = new List<RoomInformation>
            {
                new RoomInformation { RoomID = 1, RoomNumber = "101", RoomDescription = "Standard room on first floor", RoomMaxCapacity = 2, RoomStatus = 1, RoomPricePerDate = 100.00m, RoomTypeID = 1, RoomType = RoomTypes[0] },
                new RoomInformation { RoomID = 2, RoomNumber = "102", RoomDescription = "Standard room on first floor", RoomMaxCapacity = 2, RoomStatus = 1, RoomPricePerDate = 100.00m, RoomTypeID = 1, RoomType = RoomTypes[0] },
                new RoomInformation { RoomID = 3, RoomNumber = "201", RoomDescription = "Deluxe room on second floor", RoomMaxCapacity = 3, RoomStatus = 1, RoomPricePerDate = 150.00m, RoomTypeID = 2, RoomType = RoomTypes[1] },
                new RoomInformation { RoomID = 4, RoomNumber = "301", RoomDescription = "Luxury suite on third floor", RoomMaxCapacity = 4, RoomStatus = 1, RoomPricePerDate = 250.00m, RoomTypeID = 3, RoomType = RoomTypes[2] }
            };

            Customers = new List<Customer>
            {
                new Customer { CustomerID = 1, CustomerFullName = "John Doe", Telephone = "0123456789", EmailAddress = "john.doe@email.com", CustomerBirthday = new DateTime(1990, 1, 1), CustomerStatus = 1, Password = "password123" },
                new Customer { CustomerID = 2, CustomerFullName = "Jane Smith", Telephone = "0987654321", EmailAddress = "jane.smith@email.com", CustomerBirthday = new DateTime(1985, 5, 15), CustomerStatus = 1, Password = "password456" },
                new Customer { CustomerID = 3, CustomerFullName = "Mike Johnson", Telephone = "0555123456", EmailAddress = "mike.johnson@email.com", CustomerBirthday = new DateTime(1988, 8, 20), CustomerStatus = 1, Password = "password789" }
            };

            BookingReservations = new List<BookingReservation>
            {
                new BookingReservation { BookingReservationID = 1, BookingDate = DateTime.Now.AddDays(-10), TotalPrice = 300.00m, CustomerID = 1, BookingStatus = 1, Customer = Customers[0] },
                new BookingReservation { BookingReservationID = 2, BookingDate = DateTime.Now.AddDays(-5), TotalPrice = 450.00m, CustomerID = 2, BookingStatus = 1, Customer = Customers[1] }
            };

            BookingDetails = new List<BookingDetail>
            {
                new BookingDetail { BookingReservationID = 1, RoomID = 1, StartDate = DateTime.Now.AddDays(-10), EndDate = DateTime.Now.AddDays(-7), ActualPrice = 300.00m, BookingReservation = BookingReservations[0], RoomInformation = Rooms[0] },
                new BookingDetail { BookingReservationID = 2, RoomID = 3, StartDate = DateTime.Now.AddDays(-5), EndDate = DateTime.Now.AddDays(-2), ActualPrice = 450.00m, BookingReservation = BookingReservations[1], RoomInformation = Rooms[2] }
            };
        }

        public int GetNextCustomerId()
        {
            return Customers.Any() ? Customers.Max(c => c.CustomerID) + 1 : 1;
        }

        public int GetNextRoomId()
        {
            return Rooms.Any() ? Rooms.Max(r => r.RoomID) + 1 : 1;
        }

        public int GetNextBookingId()
        {
            return BookingReservations.Any() ? BookingReservations.Max(b => b.BookingReservationID) + 1 : 1;
        }
    }
}