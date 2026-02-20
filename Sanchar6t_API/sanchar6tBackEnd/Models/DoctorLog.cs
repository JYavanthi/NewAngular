using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class DoctorLog
{
    public int DoctorlogId { get; set; }

    public int? DoctorId { get; set; }

    public DateTime? LoginTime { get; set; }

    public DateTime? LogoutTime { get; set; }

    public string? Token { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
