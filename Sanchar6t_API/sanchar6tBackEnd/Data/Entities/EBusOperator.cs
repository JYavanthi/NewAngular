namespace sanchar6tBackEnd.Data.Entities
{
    public class EBusOperator
    {
        public char Flag { get; set; }
        public int BusOperatorID { get; set; }
        public string BusNo { get; set; }
        public string? BusType { get; set; }
        public string? BusSeats { get; set; }
        public string? FemaleSeatNo { get; set; }
        public string? MaleSeatNo { get; set; }
        public int CreatedBy { get; set; }
    }
}
