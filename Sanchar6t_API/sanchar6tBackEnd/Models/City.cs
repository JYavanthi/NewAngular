using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class City
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public short StateId { get; set; }

    public string? StateCode { get; set; }

    public string StateName { get; set; } = null!;

    public byte CountryId { get; set; }

    public string? CountryCode { get; set; }

    public string CountryName { get; set; } = null!;

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public decimal? WikiDataId { get; set; }

    public string? Town { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
