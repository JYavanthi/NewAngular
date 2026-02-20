using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class VwPoint
{
    public int PointId { get; set; }

    public int? Id { get; set; }

    public int? AreaId { get; set; }

    public string? Landmark { get; set; }

    public string? Town { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }

    public string PointType { get; set; } = null!;
}
