using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class PkgImpNote
{
    public int PkgImpNoteId { get; set; }

    public int PackageId { get; set; }

    public string Description { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
