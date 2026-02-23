using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class VwBusBookingDetail
{
    public int BusBooKingDetailId { get; set; }

    public int? OperatorId { get; set; }

    public int? PackageId { get; set; }

    public decimal? WkEndSeatPrice { get; set; }

    public decimal? WkDaySeatPrice { get; set; }

    public DateTime? DepartureTime { get; set; }

    public DateTime? Arrivaltime { get; set; }

    public string? Status { get; set; }

    public string? PackageName { get; set; }

    public string BusNo { get; set; } = null!;

    public string? BusSeats { get; set; }

    public string? BusType { get; set; }

    public string? FemaleSeatNo { get; set; }
}
