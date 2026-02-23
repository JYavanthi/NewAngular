using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class ArchiveJobLog
{
    public int LogId { get; set; }

    public DateTime? RunDate { get; set; }

    public string? Status { get; set; }

    public int? RowsMoved { get; set; }

    public string? ErrorMessage { get; set; }
}
