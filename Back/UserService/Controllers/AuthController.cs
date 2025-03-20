using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.DTO;
using UserService.Helpers;
using UserService.Models;

namespace UserService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;
        private readonly PasswordService _passwordHasher;

        public AuthController(ApplicationDbContext context, JwtService jwtService, PasswordService passwordService)
        {
            _context = context;
            _jwtService = jwtService;
            _passwordHasher = passwordService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register user)
        {
            try
            {
                string hashPassword = _passwordHasher.HashPassword(user.Password);
                _context.Users.Add(new User(user.Username, hashPassword, null));
                await _context.SaveChangesAsync();
                return Ok("User registered successfully.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.NameOrEmail || u.Email == user.NameOrEmail);
            if (existingUser == null || !_passwordHasher.VerifyPassword(existingUser.Password, user.Password))
                return Unauthorized("Invalid credentials.");

            var roles = _context.UserRoles
                .Where(ur => ur.UserId == existingUser.Id)
                .Select(ur => ur.Role.Name)
                .ToList();

            var permissions = _context.RolePermissions
                .Where(rp => roles.Contains(rp.Role.Name))
                .Select(rp => rp.Permission.Name)
                .ToList();

            var token = _jwtService.GenerateToken(existingUser, roles, permissions);
            return Ok(new { token });
        }

        [HttpPost("createpermission")]
        public async Task<IActionResult> CreatePermission([FromBody] List<string> permissions)
        {
            try
            {
                List<string> existingPermissions = await _context.Permissions.Select(p => p.Name).ToListAsync();
                List<Permission> newPermissions = permissions.Except(existingPermissions)
                                                 .Select(p => new Permission { Name = p })
                                                 .ToList();
                await _context.Permissions.AddRangeAsync(newPermissions);
                await _context.SaveChangesAsync();
                return Ok("Create completed");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("deletepermission")]
        public async Task<IActionResult> DeletePermission([FromBody] List<string> permissions)
        {
            try
            {
                List<Permission> permissionsToDelete = await _context.Permissions
                    .Where(p => permissions.Contains(p.Name))
                    .ToListAsync();

                if (permissionsToDelete.Any())
                {
                    _context.Permissions.RemoveRange(permissionsToDelete);
                    await _context.SaveChangesAsync();
                    return Ok("Permissions deleted successfully.");
                }
                else
                {
                    return NotFound("No permissions found to delete.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("createrole/{roleName}")]
        public async Task<IActionResult> CreateRole(string roleName, [FromBody] List<string> permissions)
        {
            try
            {
                Role role = new Role() { Name = roleName };
                Role? existingRole = _context.Roles.FirstOrDefault(p => p.Name == roleName);
                if (existingRole != null)
                {
                    return BadRequest("Role already exists");
                }
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
                return Ok("Create completed");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("deleteRole/{roleName}")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            try
            {
                Role? existingRole = await _context.Roles.FirstOrDefaultAsync(p => p.Name == roleName);
                if (existingRole == null)
                {
                    return BadRequest("Role not exists");
                }
                _context.Roles.Remove(existingRole);
                await _context.SaveChangesAsync();
                return Ok("Role deleted successfully.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("createrolepermission")]
        public async Task<IActionResult> CreateRolePermission([FromBody] List<RolePermissionDTO> values)
        {
            try
            {
                foreach (var item in values)
                {
                    Role? existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == item.RoleName);
                    Permission? existingPermission = await _context.Permissions.FirstOrDefaultAsync(p => p.Name == item.PermissionName);
                    RolePermission rolePermission = new RolePermission();
                    if (existingRole == null && existingPermission == null)
                    {
                        rolePermission = new RolePermission()
                        {
                            Role = new Role() { Name = item.RoleName },
                            Permission = new Permission() { Name = item.PermissionName }
                        };
                    }
                    else if(existingRole == null && existingPermission != null)
                    {
                        rolePermission = new RolePermission()
                        {
                            Role = new Role() { Name = item.RoleName },
                            Permission = existingPermission
                        };
                    }
                    else if(existingRole != null && existingPermission == null)
                    {
                        rolePermission = new RolePermission()
                        {
                            Role = existingRole,
                            Permission = new Permission() { Name = item.PermissionName }
                        };
                    }
                    else if (existingRole != null && existingPermission != null)
                    {
                        rolePermission = new RolePermission()
                        {
                            Role = existingRole,
                            Permission = existingPermission
                        };
                        RolePermission? rp = await _context.RolePermissions.FindAsync(existingRole.Id, existingPermission.Id);
                        if (rp != null)
                        {
                            continue;
                        }
                    }
                    await _context.RolePermissions.AddAsync(rolePermission);
                    await _context.SaveChangesAsync();
                }
                return Ok("Roles and permissions processed successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("deleterolepermission")]
        public async Task<IActionResult> DeleteRolePermission([FromBody] List<RolePermissionDTO> values)
        {
            try
            {
                foreach (var item in values)
                {
                    RolePermission? rolePermission = await _context.RolePermissions.FirstOrDefaultAsync(rp => rp.Role.Name == item.RoleName && rp.Permission.Name == item.PermissionName);
                    if (rolePermission != null)
                    {
                        _context.RolePermissions.Remove(rolePermission);
                        await _context.SaveChangesAsync();
                    }
                }
                return Ok("Roles and permissions processed successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
