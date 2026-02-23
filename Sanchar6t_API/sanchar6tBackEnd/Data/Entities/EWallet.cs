namespace sanchar6tBackEnd.Data.Entities
{
    public class EWallet
    {
        public int WalletID { get; set; }
        public int UserID { get; set; }
        public string Amount { get; set; }
        public string Type { get; set; }
        public string TransactionLimit { get; set; }
        public int CreatedBy { get; set; }
    }
}
