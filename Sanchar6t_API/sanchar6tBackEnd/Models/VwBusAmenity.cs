using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class VwBusAmenity
{
    public int BusAmenitiesId { get; set; }

    public int? BusOperatorId { get; set; }

    public int? AmenityId { get; set; }

    public string? Amname { get; set; }
}
