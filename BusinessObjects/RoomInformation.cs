using System.ComponentModel.DataAnnotations;

namespace BusinessObjects
{
    public class RoomInformation
    {
        public int RoomID { get; set; }

        [Required]
        [StringLength(50)]
        public string RoomNumber { get; set; } = string.Empty;

        [StringLength(220)]
        public string RoomDescription { get; set; } = string.Empty;

        public int RoomMaxCapacity { get; set; }

        public int RoomStatus { get; set; } // 1 Active, 2 Deleted

        public decimal RoomPricePerDate { get; set; }

        public int RoomTypeID { get; set; }

        public RoomType? RoomType { get; set; }
    }
}
