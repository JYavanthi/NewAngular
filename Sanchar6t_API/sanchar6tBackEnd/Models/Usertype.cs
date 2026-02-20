using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class Usertype
{
    public int UsertypeId { get; set; }

    public string Usertype1 { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
