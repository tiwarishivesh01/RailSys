using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailSys.Models;
using RailSys.Services;

namespace RailSys.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly RailSysDbContext _context;

        public BookingController(RailSysDbContext context)
        {
            _context = context;
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookTicket([FromQuery] int userId, [FromBody] Booking request)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null || user.Role != "Passenger")
                return Unauthorized("Only registered passengers can book tickets.");

            var train = await _context.Trains.FindAsync(request.TrainId);

            if (train == null)
                return NotFound("Train not found.");

            var schedule = await _context.Schedules
                .FirstOrDefaultAsync(s => s.TrainId == request.TrainId && s.Date == request.BookingDate);

            if (schedule == null)
                return NotFound("Train schedule not found.");

            if (request.Class == "General" && schedule.AvailableGeneralSeats <= 0)
                return BadRequest("No General seats available.");

            if (request.Class == "Ladies" && schedule.AvailableLadiesSeats <= 0)
                return BadRequest("No Ladies seats available.");

            var pnr = GeneratePNR();

            var booking = new Booking
            {
                UserId = userId,
                TrainId = request.TrainId,
                Class = request.Class,
                BookingDate = request.BookingDate,
                Status = "Booked",
                Pnr = pnr
            };

            _context.Bookings.Add(booking);

            if (request.Class == "General")
                schedule.AvailableGeneralSeats--;
            else if (request.Class == "Ladies")
                schedule.AvailableLadiesSeats--;

            await _context.SaveChangesAsync();

            var passengerEmail = user.Email;

            var bookingBody = $"Dear {user.Name},\n\nYour ticket has been booked successfully.\nPNR: {pnr}\nTrain: {train.TrainName}\nDate: {request.BookingDate:yyyy-MM-dd}\nClass: {request.Class}";

            _emailService.SendEmail(passengerEmail, "Booking Confirmation", bookingBody);


            return Ok(new
            {
                Message = "Booking successful.",
                PNR = pnr,
                BookingId = booking.BookingId
            });
        }


        private string GeneratePNR()
        {
            return Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        }

        private readonly NotificationService _emailService;

        public BookingController(RailSysDbContext context, NotificationService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelBooking([FromQuery] int userId, [FromBody] Booking request)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null || user.Role != "Passenger")
                return Unauthorized("Only registered passengers can cancel bookings.");

            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Pnr == request.Pnr && b.UserId == userId);

            if (booking == null)
                return NotFound("Booking not found.");

            if (booking.Status == "Cancelled")
                return BadRequest("Booking is already cancelled.");

            booking.Status = "Cancelled";

            var schedule = await _context.Schedules
                .FirstOrDefaultAsync(s => s.TrainId == booking.TrainId && s.Date == booking.BookingDate);

            if (schedule == null)
                return NotFound("Schedule not found for this booking.");

            if (booking.Class == "General")
                schedule.AvailableGeneralSeats++;
            else if (booking.Class == "Ladies")
                schedule.AvailableLadiesSeats++;

            await _context.SaveChangesAsync();

            var passengerEmail = user.Email;

            var cancelBody = $"Dear {user.Name},\n\nYour booking with PNR: {booking.Pnr} has been cancelled.\nRefund process has been initiated.";

            _emailService.SendEmail(passengerEmail, "Booking Cancellation Confirmation", cancelBody);

            return Ok(new
            {
                Message = "Booking cancelled successfully.",
                PNR = booking.Pnr,
                RefundStatus = "Initiated"
            });
        }

    }
}
