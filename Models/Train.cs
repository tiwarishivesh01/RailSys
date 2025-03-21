using System;
using System.Collections.Generic;

namespace RailSys.Models;

public partial class Train
{
    public int TrainId { get; set; }

    public string TrainName { get; set; } = null!;

    public string Source { get; set; } = null!;

    public string Destination { get; set; } = null!;

    public int TotalSeats { get; set; }

    public int GeneralQuotaSeats { get; set; }

    public int LadiesQuotaSeats { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
