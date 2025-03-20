using System.ComponentModel.DataAnnotations;

namespace UserService.Models
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<RolePermission> RolePermissions { get; set; } = new();

        public Permission() { }
        public Permission(string name) { Name = name; }
    }
}
