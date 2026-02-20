using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class PassengerDtl
{
    public int PassengerDtlId { get; set; }

    public int PassengerId { get; set; }

    public string? FristName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public decimal? ContactNo { get; set; }

    public decimal? AadharNo { get; set; }

    public decimal? PanNo { get; set; }

    public string? BloodGroup { get; set; }

    public string? Gst { get; set; }

    public DateTime? Dob { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
