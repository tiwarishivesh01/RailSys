using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailSys.Models;

namespace RailSys.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly RailSysDbContext _context;

        public AuthController(RailSysDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser != null)
                return BadRequest("Email is already registered.");

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Registration successful.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User login)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == login.Email && u.Password == login.Password);

            if (user == null)
                return Unauthorized("Invalid email or password.");

            return Ok(new
            {
                Message = "Login successful.",
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            });
        }

        [HttpPost("admin/login")]
        public async Task<IActionResult> AdminLogin([FromBody] Admin login)
        {
            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Email == login.Email && a.Password == login.Password);

            if (admin == null)
                return Unauthorized("Invalid admin credentials.");

            return Ok(new
            {
                Message = "Admin login successful.",
                AdminId = admin.AdminId,
                Name = admin.Name,
                Email = admin.Email
            });
        }

    }
}
