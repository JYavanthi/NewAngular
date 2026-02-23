using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class BookingTicketEmailLog
{
    public int BookingTicketEmailLogId { get; set; }

    public int BookingId { get; set; }

    public string TicketNumber { get; set; } = null!;

    public int UserId { get; set; }

    public string EmailTo { get; set; } = null!;

    public string EmailType { get; set; } = null!;

    public string EmailStatus { get; set; } = null!;

    public int RetryCount { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? LastSentDate { get; set; }

    public string? ErrorMessage { get; set; }
}
