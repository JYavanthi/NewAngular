namespace sanchar6tBackEnd.Data.Entities
{
    public class Ecities
    {
        public char Flag { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public short state_id { get; set; }
        public byte country_id { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
        public decimal? wikiDataId { get; set; }
        public int CreatedBy { get; set; }

    }
}
