using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class Amenity
{
    public int AmenityId { get; set; }

    public string? Amicon { get; set; }

    public string? Amname { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }

    public virtual ICollection<BusAmenity> BusAmenities { get; set; } = new List<BusAmenity>();
}
