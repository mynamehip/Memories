namespace UserService.Models
{
    public class RolePermission
    {
        public int RoleId { get; set; }
        public Role Role { get; set; } = new();
        public int PermissionId { get; set; }
        public Permission Permission { get; set; } = new();

        public RolePermission() { }
        public RolePermission(int roleId, int permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }
    }
}
