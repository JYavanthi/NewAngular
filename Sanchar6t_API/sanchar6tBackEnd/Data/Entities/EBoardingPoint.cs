namespace sanchar6tBackEnd.Data.Entities
{
    public class EBoardingPoint
    {
        public char Flag { get; set; }
        public int BoardingPointID { get; set; }
        public int? id { get; set; }
        public int? AreaID { get; set; }
        public string? Landmark { get; set; }
        public string? Town { get; set; }
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }
        public int CreatedBy { get; set; }
    }
}
