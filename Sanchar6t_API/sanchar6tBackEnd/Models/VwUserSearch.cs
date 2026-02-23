using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class VwUserSearch
{
    public int UserId { get; set; }

    public DateTime? SearchedDate { get; set; }

    public int From { get; set; }

    public int To { get; set; }

    public string? ModeOfTransport { get; set; }

    public string? Operator { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
