using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class VwBusBookingDetail
{
    public int BusBooKingDetailId { get; set; }

    public int? UserId { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public int? OperatorId { get; set; }

    public int? AgentId { get; set; }

    public int? BoardingPoint { get; set; }

    public int? DroppingPoint { get; set; }

    public int? ScheduleId { get; set; }

    public DateTime? DepartureTime { get; set; }

    public DateTime? ArrivalTime { get; set; }

    public string? BusNum { get; set; }

    public string? Status { get; set; }

    public int CreatedBy { get; set; }

    public string CreatedByName { get; set; } = null!;

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public string ModifiedByName { get; set; } = null!;

    public DateTime? ModifiedDt { get; set; }
}
