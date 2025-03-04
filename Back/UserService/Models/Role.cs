namespace UserService.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<UserRole> UserRoles { get; set; } = new();
        public List<RolePermission> RolePermissions { get; set; } = new();
    }
}
