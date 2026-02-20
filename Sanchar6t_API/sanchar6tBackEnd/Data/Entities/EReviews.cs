namespace sanchar6tBackEnd.Data.Entities
{
    public class EReviews
    {
        public char Flag { get; set; }
        public int ReviewID { get; set; }
        public int UserID { get; set; }
        public int BusBooKingDetailID { get; set; }
        public int? Rating { get; set; }
        public string? Description { get; set; }
        public int CreatedBy { get; set; }
    }
}
