using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class TicketAudit
{
    public int TicketAuditId { get; set; }

    public string? TicketNo { get; set; }

    public int? BusBookingSeatId { get; set; }

    public int? BusBookingDetailsId { get; set; }

    public string? SeatNo { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreatedDt { get; set; }
}
