using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class PkgItinerary
{
    public int PkgItineraryId { get; set; }

    public int PackageId { get; set; }

    public int Day { get; set; }

    public DateTime FromTime { get; set; }

    public DateTime ToTime { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreateDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
