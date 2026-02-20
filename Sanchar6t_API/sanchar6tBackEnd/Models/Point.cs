using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class Point
{
    public int PointId { get; set; }

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

    public virtual Area? Area { get; set; }

    public virtual ICollection<BusPoint> BusPoints { get; set; } = new List<BusPoint>();
}
