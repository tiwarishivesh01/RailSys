using System;
using System.Collections.Generic;

namespace RailSys.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int UserId { get; set; }

    public int TrainId { get; set; }

    public int ScheduleId { get; set; }

    public int SeatCount { get; set; }

    public string Status { get; set; } = null!;

    public DateTime BookingDate { get; set; }

    public string Class {get; set; } = "General";

    public string Pnr { get; set; } = "";

    public virtual Schedule Schedule { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
