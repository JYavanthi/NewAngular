using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class BusPoint
{
    public int BusPointId { get; set; }

    public int? BusBooKingDetailId { get; set; }

    public string? Status { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }

    public string PointType { get; set; } = null!;

    public int? PointId { get; set; }

    public DateTime? ArrivalTime { get; set; }

    public DateTime? DepartureTime { get; set; }

    public virtual Point? Point { get; set; }
}
