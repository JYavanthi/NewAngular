using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class BusBookingDetail
{
    public int BusBooKingDetailId { get; set; }

    public int? OperatorId { get; set; }

    public int? PackageId { get; set; }

    public decimal? WkEndSeatPrice { get; set; }

    public decimal? WkDaySeatPrice { get; set; }

    public DateTime? DepartureTime { get; set; }

    public DateTime? Arrivaltime { get; set; }

    public string? Status { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
