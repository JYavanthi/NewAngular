using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class Package
{
    public int PackageId { get; set; }

    public string? PackageName { get; set; }

    public int State { get; set; }

    public int Country { get; set; }

    public int From { get; set; }

    public int To { get; set; }

    public int? Noofdays { get; set; }

    public string? Shortdescription { get; set; }

    public string? Description { get; set; }

    public string? AdditionalNotes { get; set; }

    public decimal? PackagePrice { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
