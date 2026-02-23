using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class VwBusBoardingAndDroppingPoint
{
    public string PointType { get; set; } = null!;

    public int? BusBooKingDetailId { get; set; }

    public string? PointName { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public string AreaName { get; set; } = null!;

    public string Pincode { get; set; } = null!;

    public DateTime? Time { get; set; }
}
