using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class Attachment
{
    public int AttachmentId { get; set; }

    public int UserId { get; set; }

    public int? PackageId { get; set; }

    public string? Section { get; set; }

    public int? Sortorder { get; set; }

    public string AttachmentName { get; set; } = null!;

    public string AttachmentFile { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
