using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int? UserId { get; set; }

    public int? BookingdtlsId { get; set; }

    public int? Amount { get; set; }

    public string? PaymentMode { get; set; }

    public string? TransactionId { get; set; }

    public string? TransactionResponse { get; set; }

    public string? TransactionCode { get; set; }

    public string? PaymentStatus { get; set; }

    public string? ErrorCode { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }

    public int? BusBookingSeatId { get; set; }
}
