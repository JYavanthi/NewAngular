using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class ServiceDtl
{
    public int ServiceId { get; set; }

    public int? PackageId { get; set; }

    public string? Servicename { get; set; }

    public string? BusType { get; set; }

    public TimeSpan? Departure { get; set; }

    public string? Duration { get; set; }

    public TimeSpan? Arrival { get; set; }

    public int? Fare { get; set; }

    public string? WdFrom { get; set; }

    public string? WdTo { get; set; }

    public int? WdFare { get; set; }

    public string? WeFrom { get; set; }

    public string? WeTo { get; set; }

    public int? WeFare { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
