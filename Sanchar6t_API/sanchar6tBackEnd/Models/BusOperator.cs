using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class BusOperator
{
    public int BusOperatorId { get; set; }

    public string BusNo { get; set; } = null!;

    public string? BusType { get; set; }

    public string? BusSeats { get; set; }

    public string? FemaleSeatNo { get; set; }

    public string? MaleSeatNo { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }

    public string? SourceSystem { get; set; }

    public virtual ICollection<BusAmenity> BusAmenities { get; set; } = new List<BusAmenity>();
}
