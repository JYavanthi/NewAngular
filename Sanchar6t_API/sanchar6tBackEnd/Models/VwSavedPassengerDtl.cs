using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class VwSavedPassengerDtl
{
    public int PassengerDtlId { get; set; }

    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string ContactNo { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string? AadharNo { get; set; }

    public string? PancardNo { get; set; }

    public string? BloodGroup { get; set; }

    public bool PrimaryUser { get; set; }

    public DateTime? Dob { get; set; }

    public string? FoodPref { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedDt { get; set; }
}
