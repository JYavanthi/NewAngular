namespace sanchar6tBackEnd.Data.Entities
{
    public class EArea
    {
        public char Flag { get; set; }
        public int AreaID { get; set; }
        public string AreaName { get; set; }
        public string Pincode { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public int? CityID { get; set; }
        public int CreatedBy { get; set; }
    }
}
