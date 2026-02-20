using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class VwPkgInclude
{
    public int PkgIncludeId { get; set; }

    public int PackageId { get; set; }

    public string Description { get; set; } = null!;

    public bool IsIncluded { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public string CreatedByName { get; set; } = null!;

    public DateTime CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public string ModifiedbyByName { get; set; } = null!;

    public DateTime? ModifiedDt { get; set; }
}
