using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class Area
{
    public int AreaId { get; set; }

    public string AreaName { get; set; } = null!;

    public string Pincode { get; set; } = null!;

    public double? Longitude { get; set; }

    public double? Latitude { get; set; }

    public int? CityId { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }

    public virtual ICollection<Point> Points { get; set; } = new List<Point>();
}
