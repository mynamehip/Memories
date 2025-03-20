using System.ComponentModel.DataAnnotations;

namespace UserService.DTO
{
    public class RolePermissionDTO
    {
        [Required]
        public string RoleName { get; set; } = string.Empty;
        [Required]
        public string PermissionName { get; set; } = string.Empty;
    }
}
