using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class UserSecurity
{
    public int UserSecurityId { get; set; }

    public int UserId { get; set; }

    public string Password { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
