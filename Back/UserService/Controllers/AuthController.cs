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
        public async Task<IActionResult> Register([FromBody] LoginUser user)
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
        public async Task<IActionResult> Login([FromBody] LoginUser user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
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
    }
}
