using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class VwBookedSeat
{
    public int UserId { get; set; }

    public int SeatNo { get; set; }

    public int SeatPriece { get; set; }

    public string Gender { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
