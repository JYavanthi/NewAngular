namespace sanchar6tBackEnd.Data.Entities
{
    public class EWalletTransaction
    {
        public char Flag { get; set; }
        public int WalletTrnsnID { get; set; }
        public int UserID { get; set; }
        public string Amount { get; set; }
        public DateTime Date { get; set; }
        public string Mode { get; set; }
        public string TransactionNumber { get; set; }
        public string ErrorCode { get; set; }
        public string TransactionCode { get; set; }
        public string Message { get; set; }
        public int CreatedBy { get; set; }

    }
}
