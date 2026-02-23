using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class VwBusTripSeatAvailability
{
    public int BusBooKingDetailId { get; set; }

    public int BusOperatorId { get; set; }

    public string BusNo { get; set; } = null!;

    public string? TotalSeats { get; set; }

    public int? BookedSeats { get; set; }

    public int? RemainingSeats { get; set; }

    public int? PackageId { get; set; }

    public DateTime? DepartureTime { get; set; }

    public DateTime? ArrivalTime { get; set; }

    public string? Status { get; set; }
}
