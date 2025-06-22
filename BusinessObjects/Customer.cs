using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects
{
    public class Customer
    {
        public int CustomerID { get; set; }

        [Required]
        [StringLength(50)]
        public string CustomerFullName { get; set; } = string.Empty;

        [StringLength(12)]
        public string Telephone { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;

        public DateTime CustomerBirthday { get; set; }

        public int CustomerStatus { get; set; } // 1 Active, 2 Deleted

        [Required]
        [StringLength(50)]
        public string Password { get; set; } = string.Empty;
    }
}
