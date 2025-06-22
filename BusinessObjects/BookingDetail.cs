using System;

namespace BusinessObjects
{
    public class BookingDetail
    {
        public int BookingReservationID { get; set; }
        public int RoomID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal ActualPrice { get; set; }
        public BookingReservation? BookingReservation { get; set; }
        public RoomInformation? RoomInformation { get; set; }
    }
}