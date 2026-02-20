using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class PkimportantNote
{
    public int PackageId { get; set; }

    public int PkimportantNoteId { get; set; }

    public string Description { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
