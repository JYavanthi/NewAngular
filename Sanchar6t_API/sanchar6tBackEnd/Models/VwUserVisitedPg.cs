using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class VwUserVisitedPg
{
    public int UserVisitedPgId { get; set; }

    public int UserId { get; set; }

    public DateTime? VisitedPgTimeFrom { get; set; }

    public DateTime? VisitedPgTimeTo { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
