using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailSys.Models;

namespace RailSys.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainController : ControllerBase
    {
        private readonly RailSysDbContext _context;

        public TrainController(RailSysDbContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTrain([FromBody] Train train, [FromQuery] int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null || user.Role != "Admin")
                return Unauthorized("Only admins can perform this action.");

            _context.Trains.Add(train);
            await _context.SaveChangesAsync();

            return Ok("Train created successfully.");
        }

    }
}