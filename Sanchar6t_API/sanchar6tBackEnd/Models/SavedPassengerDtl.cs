using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class SavedPassengerDtl
{
    public int PassengerDtlId { get; set; }

    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? ContactNo { get; set; }

    public string? Gender { get; set; }

    public string? AadharNo { get; set; }

    public string? PancardNo { get; set; }

    public string? BloodGroup { get; set; }

    public bool PrimaryUser { get; set; }

    public string? Age { get; set; }

    public string? Address { get; set; }

    public string? AlternativeNumber { get; set; }

    public string? Remarks { get; set; }

    public string? FoodPref { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
