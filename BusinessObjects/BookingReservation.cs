using System;
using System.Collections.ObjectModel;

namespace BusinessObjects
{
    public class BookingReservation
    {
        public int BookingReservationID { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int CustomerID { get; set; }
        public int BookingStatus { get; set; } // 1 Active, 2 Cancelled
        public Customer? Customer { get; set; }
        public ObservableCollection<BookingDetail> BookingDetails { get; set; } = new ObservableCollection<BookingDetail>();
    }
}