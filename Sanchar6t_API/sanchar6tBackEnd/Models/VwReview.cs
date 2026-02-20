using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class VwReview
{
    public int ReviewId { get; set; }

    public int BusBooKingDetailId { get; set; }

    public int UserId { get; set; }

    public int? Rating { get; set; }

    public string? Description { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
