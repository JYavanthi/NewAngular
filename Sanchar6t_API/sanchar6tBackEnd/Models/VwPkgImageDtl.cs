using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class VwPkgImageDtl
{
    public int PackageId { get; set; }

    public int PkgImageId { get; set; }

    public string? PkgImage { get; set; }

    public string? PkgSection { get; set; }

    public string? Heading { get; set; }

    public string? SubHeading { get; set; }

    public string? BtnName { get; set; }

    public string? BtnUrl { get; set; }

    public int CreatedBy { get; set; }

    public string CreatedByName { get; set; } = null!;

    public DateTime CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public string ModifiedbyByName { get; set; } = null!;

    public DateTime? ModifiedDt { get; set; }
}
