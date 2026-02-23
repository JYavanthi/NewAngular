using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class UserFeedback
{
    public int UserFeedbackId { get; set; }

    public string Name { get; set; } = null!;

    public string ContactNo { get; set; } = null!;

    public string EmailId { get; set; } = null!;

    public string UserFeedback1 { get; set; } = null!;

    public int PackageId { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
