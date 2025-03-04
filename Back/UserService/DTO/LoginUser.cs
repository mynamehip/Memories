using System.ComponentModel.DataAnnotations;

namespace UserService.DTO
{
    public class LoginUser
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
