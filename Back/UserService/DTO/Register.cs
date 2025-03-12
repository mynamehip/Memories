using System.ComponentModel.DataAnnotations;

namespace UserService.DTO
{
    public class Register
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
