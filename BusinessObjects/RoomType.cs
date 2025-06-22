using System.ComponentModel.DataAnnotations;

namespace BusinessObjects
{
    public class RoomType
    {
        public int RoomTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string RoomTypeName { get; set; } = string.Empty;

        [StringLength(250)]
        public string TypeDescription { get; set; } = string.Empty;

        [StringLength(250)]
        public string TypeNote { get; set; } = string.Empty;
    }
}
