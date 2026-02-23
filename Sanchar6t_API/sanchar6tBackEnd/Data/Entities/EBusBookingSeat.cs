namespace sanchar6tBackEnd.Data.Entities
{
    public class EBusBookingSeat
    {
        public char Flag { get; set; }
        public int BusBookingSeatID { get; set; }

        public int? UserID { get; set; }

        public bool? ForSelf { get; set; }  

        public bool? IsPrimary { get; set; }

        public string? SeatNo { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }


        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? ContactNo { get; set; }

        public string? Gender { get; set; }
        public string? AadharNo { get; set; }
        public string? PancardNo { get; set; }
        public string? BloodGroup { get; set; }
        public DateTime? DOB { get; set; }
        public string? FoodPref { get; set; }
        public int CreatedBy { get; set; }

        public DateTime? CreatedDt { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDt { get; set; }
        public int? BusBookingDetailsId { get; set; }

        public int? BusOperatorId { get; set; }

        public bool? Disabled { get; set; }

        public bool? Pregnant { get; set; }

        public string? RegisteredCompanyNumber { get; set; }

        public string? RegisteredCompanyName { get; set; }

        public string? DrivingLicence { get; set; }

        public string? PassportNo { get; set; }

        public string? RationCard { get; set; }

        public string? VoterId { get; set; }

        public string? Others { get; set; }

        public string? SavePassengerDetails { get; set; }

        public bool? Nri { get; set; }

        public string? Status { get; set; }

        public string? LockStatus { get; set; }

        public int? CancelledBy { get; set; }

        public DateTime? CancelledDate { get; set; }

        public string? PaymentStatus { get; set; }

        public DateTime? JourneyDate { get; set; }

        public string? TicketNo { get; set; }
    }
}
