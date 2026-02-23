namespace sanchar6tBackEnd.Data.Entities
{
    public class EUserVisitedPgs
    {
        public char Flag { get; set; }

        public int UserVisitedPgID { get; set; }

        public int UserID { get; set; }
         
        public DateTime? VisitedPgTimeFrom { get; set; }

        public DateTime? VisitedPgTimeTo { get; set; }
        public int CreatedBy { get; set; }
    }
}
