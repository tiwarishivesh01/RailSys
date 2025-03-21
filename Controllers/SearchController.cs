using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailSys.Models;

namespace RailSys.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly RailSysDbContext _context;

        public SearchController(RailSysDbContext context)
        {
            _context = context;
        }

        [HttpGet("trains")]
        public async Task<IActionResult> SearchTrains([FromQuery] string source, [FromQuery] string destination)
        {
            var trains = await _context.Trains
                .Where(t => t.Source == source && t.Destination == destination)
                .ToListAsync();

            if (trains == null || trains.Count == 0)
            { return NotFound("No trains found for the given route."); }

            return Ok(trains);
        }

        [HttpGet("schedules")]
        public async Task<IActionResult> SearchSchedules([FromQuery] string trainName)
        {
            var schedules = await _context.Schedules
                .Include(s => s.Train)
                .Where(s => s.Train.TrainName.ToLower() == trainName.ToLower())
                .Select(s => new
                {
                    s.ScheduleId,
                    s.TrainId,
                    s.Train.TrainName,
                    s.Date,
                    s.AvailableGeneralSeats,
                    s.AvailableLadiesSeats
                })
                .ToListAsync();

            if (schedules == null || schedules.Count == 0)
                return NotFound("No schedules found for the given train.");

            return Ok(schedules);
        }


    }
}
