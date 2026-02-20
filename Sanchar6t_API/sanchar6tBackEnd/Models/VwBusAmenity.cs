using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class VwBusAmenity
{
    public int BusAmenitiesId { get; set; }

    public int? BusOperatorId { get; set; }

    public int? AmenityId { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
