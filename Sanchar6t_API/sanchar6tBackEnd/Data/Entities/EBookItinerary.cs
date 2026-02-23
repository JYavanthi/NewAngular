using System.Diagnostics.Contracts;

namespace sanchar6tBackEnd.Data.Entities
{
    public class EBookItinerary
    {
        public char Flag { get; set; }
        public int ItineraryID { get; set; }  
        public int PackageID { get; set; }
        public string Image { get; set; }
        public int Day { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }

    }
}
