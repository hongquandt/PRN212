using BusinessObjects;
using DataAccessLayer.DataContext;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly InMemoryDataContext _context;

        public BookingRepository()
        {
            _context = InMemoryDataContext.Instance;
        }

        public List<BookingReservation> GetAllBookings()
        {
            return _context.BookingReservations
                .Where(b => b.BookingStatus == 1)
                .ToList()
                .Select(b =>
                {
                    b.BookingDetails = new ObservableCollection<BookingDetail>(
                        _context.BookingDetails.Where(bd => bd.BookingReservationID == b.BookingReservationID));
                    return b;
                }).ToList();
        }

        public List<BookingReservation> GetBookingsByCustomerId(int customerId)
        {
            var bookings = _context.BookingReservations
                .Where(b => b.CustomerID == customerId && b.BookingStatus == 1)
                .ToList()
                .Select(b =>
                {
                    b.BookingDetails = new ObservableCollection<BookingDetail>(
                        _context.BookingDetails.Where(bd => bd.BookingReservationID == b.BookingReservationID));
                    return b;
                }).ToList();
            System.Diagnostics.Debug.WriteLine($"[05:06 PM +07, 22/06/2025] GetBookingsByCustomerId for {customerId} returned {bookings.Count} records");
            return bookings;
        }

        public List<BookingReservation> GetBookingsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _context.BookingReservations
                .Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate && b.BookingStatus == 1)
                .OrderByDescending(b => b.BookingDate)
                .ToList()
                .Select(b =>
                {
                    b.BookingDetails = new ObservableCollection<BookingDetail>(
                        _context.BookingDetails.Where(bd => bd.BookingReservationID == b.BookingReservationID));
                    return b;
                }).ToList();
        }

        public BookingReservation? GetBookingById(int id)
        {
            var booking = _context.BookingReservations.FirstOrDefault(b => b.BookingReservationID == id && b.BookingStatus == 1);
            if (booking != null)
            {
                booking.BookingDetails = new ObservableCollection<BookingDetail>(
                    _context.BookingDetails.Where(bd => bd.BookingReservationID == booking.BookingReservationID));
            }
            return booking;
        }

        public void AddBooking(BookingReservation booking)
        {
            booking.BookingReservationID = _context.GetNextBookingId();
            booking.BookingStatus = 1;
            booking.Customer = _context.Customers.FirstOrDefault(c => c.CustomerID == booking.CustomerID) ?? booking.Customer;
            _context.BookingReservations.Add(booking);
            System.Diagnostics.Debug.WriteLine($"[05:06 PM +07, 22/06/2025] Added BookingReservation ID: {booking.BookingReservationID} to context");
        }

        public void UpdateBooking(BookingReservation booking)
        {
            var existingBooking = _context.BookingReservations.FirstOrDefault(b => b.BookingReservationID == booking.BookingReservationID);
            if (existingBooking != null)
            {
                existingBooking.BookingDate = booking.BookingDate;
                existingBooking.TotalPrice = booking.TotalPrice;
                existingBooking.CustomerID = booking.CustomerID;
                existingBooking.Customer = _context.Customers.FirstOrDefault(c => c.CustomerID == booking.CustomerID) ?? existingBooking.Customer;
                System.Diagnostics.Debug.WriteLine($"[05:06 PM +07, 22/06/2025] Updated BookingReservation ID: {booking.BookingReservationID}");
            }
        }

        public void DeleteBooking(int id)
        {
            var booking = _context.BookingReservations.FirstOrDefault(b => b.BookingReservationID == id);
            if (booking != null)
            {
                booking.BookingStatus = 2; // Cancelled
                System.Diagnostics.Debug.WriteLine($"[05:06 PM +07, 22/06/2025] Marked Booking ID: {id} as cancelled");
            }
        }
    }
}