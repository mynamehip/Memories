using System.ComponentModel.DataAnnotations;

namespace UserService.DTO
{
    public class Login
    {
        [Required]
        public string NameOrEmail { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
