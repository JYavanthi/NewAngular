using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class SeatLock
{
    public int SeatLockId { get; set; }

    public int? BusBookingDetailsId { get; set; }

    public int? BusOperatorId { get; set; }

    public string? SeatNo { get; set; }

    public string? SessionId { get; set; }

    public DateTime? LockedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public DateTime? JourneyDate { get; set; }
}
