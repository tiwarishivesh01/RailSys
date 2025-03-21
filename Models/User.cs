using System;
using System.Collections.Generic;

namespace RailSys.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = "Passenger";

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
