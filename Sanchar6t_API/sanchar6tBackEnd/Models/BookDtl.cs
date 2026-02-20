using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class BookDtl
{
    public int BookDtlsId { get; set; }

    public string? Description { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
