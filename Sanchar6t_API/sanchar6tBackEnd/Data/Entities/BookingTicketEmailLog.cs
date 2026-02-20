using System;

namespace sanchar6tBackEnd.Data.Entities
{
    public class BookingTicketEmailLog
    {
        public int BookingTicketEmailLogId { get; set; }
        public int BookingId { get; set; }
        public string TicketNumber { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string EmailTo { get; set; } = string.Empty;
        public string EmailType { get; set; } = string.Empty;
        public string EmailStatus { get; set; } = string.Empty;
        public int RetryCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastSentDate { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
