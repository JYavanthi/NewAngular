namespace sanchar6tBackEnd.Models
{
    public class CityPairs
    {
        public int origin_id { get; set; }
        public int destination_id { get; set; }
        public List<int> travel_ids { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDt { get; set; }

    }
}
