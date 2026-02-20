using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class User
{
    public int UserId { get; set; }

    public int UserType { get; set; }

    public string Status { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
