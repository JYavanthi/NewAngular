using System;

namespace sanchar6tBackEnd.Data.Entities
{
    public class EBusBookingDetails
    {
        public char Flag { get; set; }
        public int? BusBooKingDetailID { get; set; }
        public int? UserID { get; set; }
        public DateTime? FromDate { get; set; }  
        public DateTime? ToDate { get; set; }
        public Decimal? OperatorID { get; set; }
        public int? AgentID { get; set; }
        public int? BoardingPoint { get; set; }
        public int? DroppingPoint { get; set; }
        public int? ScheduleID { get; set; }
        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public string? BusNum { get; set; }
        public decimal? SeatPrice { get; set; }
        public string? Status { get; set; }
        public int CreatedBy { get; set; }

    }
}
