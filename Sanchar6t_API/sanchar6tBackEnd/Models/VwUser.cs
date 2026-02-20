using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class VwUser
{
    public int UserId { get; set; }

    public int UserTypeId { get; set; }

    public string? Usertype { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? Gender { get; set; }

    public string? Email { get; set; }

    public string? ContactNo { get; set; }

    public string? Password { get; set; }

    public string Status { get; set; } = null!;

    public int? CreatedBy { get; set; }

    public string CreatedByName { get; set; } = null!;

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
