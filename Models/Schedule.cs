using System;
using System.Collections.Generic;

namespace RailSys.Models;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public int TrainId { get; set; }

    public DateTime Date { get; set; }

    public int AvailableGeneralSeats { get; set; }

    public int AvailableLadiesSeats { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Train Train { get; set; } = null!;
}
