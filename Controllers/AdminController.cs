using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailSys.Models;
using RailSys.Services;

namespace RailSys.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly RailSysDbContext _context;

        public AdminController(RailSysDbContext context)
        {
            _context = context;
        }

        [HttpGet("trains")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllTrains()
        {
            var trains = await _context.Trains.ToListAsync();
            return Ok(trains);
        }

        [HttpGet("trains/{name}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTrainById(int name)
        {
            var train = await _context.Trains.FindAsync(name);

            if (train == null)
                return NotFound("Train not found.");

            return Ok(train);
        }

        [HttpPost("trains")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTrain([FromBody] Train train)
        {
            _context.Trains.Add(train);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Train created successfully.", TrainId = train.TrainId });
        }

        [HttpPut("trains/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTrain(int id, [FromBody] Train updatedTrain)
        {
            var train = await _context.Trains.FindAsync(id);

            if (train == null)
                return NotFound("Train not found.");

            train.TrainName = updatedTrain.TrainName;
            train.Source = updatedTrain.Source;
            train.Destination = updatedTrain.Destination;

            await _context.SaveChangesAsync();

            return Ok("Train updated successfully.");
        }

        [HttpDelete("trains/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTrain(int id)
        {
            var train = await _context.Trains.FindAsync(id);

            if (train == null)
                return NotFound("Train not found.");

            _context.Trains.Remove(train);
            await _context.SaveChangesAsync();

            return Ok("Train deleted successfully.");
        }

        [HttpGet("schedules")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllSchedules()
        {
            var schedules = await _context.Schedules.ToListAsync();
            return Ok(schedules);
        }

        [HttpGet("schedules/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetScheduleById(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);

            if (schedule == null)
                return NotFound("Schedule not found.");

            return Ok(schedule);
        }

        [HttpPost("schedules")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSchedule([FromBody] Schedule schedule)
        {
            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Schedule created successfully.", ScheduleId = schedule.ScheduleId });
        }

        [HttpPut("schedules/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSchedule(int id, [FromBody] Schedule updatedSchedule)
        {
            var schedule = await _context.Schedules.FindAsync(id);

            if (schedule == null)
                return NotFound("Schedule not found.");

            schedule.TrainId = updatedSchedule.TrainId;
            schedule.Date = updatedSchedule.Date;
            schedule.AvailableGeneralSeats = updatedSchedule.AvailableGeneralSeats;
            schedule.AvailableLadiesSeats = updatedSchedule.AvailableLadiesSeats;

            await _context.SaveChangesAsync();

            return Ok("Schedule updated successfully.");
        }

        [HttpDelete("schedules/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);

            if (schedule == null)
                return NotFound("Schedule not found.");

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();

            return Ok("Schedule deleted successfully.");
        }

    }
}
