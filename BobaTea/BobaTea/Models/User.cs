using System;
using System.ComponentModel.DataAnnotations;

namespace BobaTea.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [Phone]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        public string AvatarUrl { get; set; }

        [StringLength(20)]
        public string Role { get; set; } = "User";
    }
}
