using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class BusBlockedSeat
{
    public int Id { get; set; }

    public int BusBookingDetailId { get; set; }

    public DateTime JourneyDate { get; set; }

    public string? BlockedSeats { get; set; }

    public DateTime? BlockExpiresAt { get; set; }

    public DateTime? CreatedDt { get; set; }
}
