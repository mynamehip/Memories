using System.ComponentModel.DataAnnotations;

namespace UserService.Models
{
    public class UserInfo
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(100)]
        public string? Name { get; set; }
        [MaxLength(100)]
        public string? Email { get; set; }
        [MaxLength(11)]
        public string? Phone { get; set; }
        public DateTime? Birth { get; set; }
        [MaxLength(200)]
        public string? Address { get; set; }
    }
}
