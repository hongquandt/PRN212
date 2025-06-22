using BusinessObjects;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Repositories
{
    public interface IBookingRepository
    {
        List<BookingReservation> GetAllBookings();
        List<BookingReservation> GetBookingsByCustomerId(int customerId);
        List<BookingReservation> GetBookingsByDateRange(DateTime startDate, DateTime endDate);
        BookingReservation? GetBookingById(int id);
        void AddBooking(BookingReservation booking);
        void UpdateBooking(BookingReservation booking);
        void DeleteBooking(int id);
    }
}