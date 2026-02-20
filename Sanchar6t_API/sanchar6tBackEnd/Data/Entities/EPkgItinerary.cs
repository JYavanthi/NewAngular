namespace sanchar6tBackEnd.Data.Entities
{
    public class EPkgItinerary
    {
        public char Flag { get; set; }
        public int PkgItineraryID { get; set; }
        public int PackageID { get; set; }
        public string Day { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }
}  