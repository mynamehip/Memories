namespace UserService.Models
{
    public class UserRole
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = new();
        public int RoleId { get; set; }
        public Role Role { get; set; } = new();
    }
}
