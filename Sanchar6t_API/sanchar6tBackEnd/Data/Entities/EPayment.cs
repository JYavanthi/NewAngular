namespace sanchar6tBackEnd.Data.Entities
{
    public class EPayment
    {
        public char Flag { get; set; }
        public int PaymentID { get; set; }
        public int? UserID { get; set; }
        public int? BookingdtlsID { get; set; }
        public decimal? Amount { get; set; }
        public string? PaymentMode { get; set; }
        public string? TransactionID { get; set; }
        public string? TransactionResponse { get; set; }
        public string? TransactionCode { get; set; }        
        public string? PaymentStatus { get; set; }
        public string? ErrorCode { get; set; }
        public int CreatedBy { get; set; }
       
    }
}
