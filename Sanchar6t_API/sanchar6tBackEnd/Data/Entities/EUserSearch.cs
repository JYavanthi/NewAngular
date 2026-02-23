namespace sanchar6tBackEnd.Data.Entities
{
    public class EUserSearch
    {
        public char Flag { get; set; }
        public int UserID { get; set; }
        public DateTime? SearchedDate { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public string? ModeOfTransport { get; set; }
        public string? Operator { get; set; }
        public int CreatedBy { get; set; }

    }
}
