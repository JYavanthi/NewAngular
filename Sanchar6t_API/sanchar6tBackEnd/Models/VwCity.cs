using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class VwCity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public short StateId { get; set; }

    public byte CountryId { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public decimal? WikiDataId { get; set; }

    public int? CreatedBy { get; set; }

    public string CreatedByName { get; set; } = null!;

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
