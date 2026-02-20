using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class BookItinerary
{
    public int ItineraryId { get; set; }

    public int? PackageId { get; set; }

    public string? Image { get; set; }

    public int? Day { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreateDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
