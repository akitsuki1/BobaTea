using System.ComponentModel.DataAnnotations;

namespace BobaTea.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Mật khẩu không trùng khớp.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}

