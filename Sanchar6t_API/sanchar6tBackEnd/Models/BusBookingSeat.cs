using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class BusBookingSeat
{
    public int BusBookingSeatId { get; set; }

    public int? UserId { get; set; }

    public bool? ForSelf { get; set; }

    public int? IsPrimary { get; set; }

    public string? SeatNo { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? ContactNo { get; set; }

    public string? Gender { get; set; }

    public string AadharNo { get; set; } = null!;

    public string PancardNo { get; set; } = null!;

    public string? BloodGroup { get; set; }

    public DateTime? Dob { get; set; }

    public string? FoodPref { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
