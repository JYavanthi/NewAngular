using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class BusBoardingPoint
{
    public int BusBoardingPointId { get; set; }

    public int? BusBooKingDetailId { get; set; }

    public int? BoardingPoint { get; set; }

    public DateTime? BparrivalTime { get; set; }

    public int? DroppingPoint { get; set; }

    public DateTime? DropTime { get; set; }

    public string? Status { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
