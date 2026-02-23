using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class PkgOffer
{
    public int PkgOfferId { get; set; }

    public int PackageId { get; set; }

    public decimal Price { get; set; }

    public bool OfferPrice { get; set; }

    public DateTime EffectiveDate { get; set; }

    public bool WeekDay { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
