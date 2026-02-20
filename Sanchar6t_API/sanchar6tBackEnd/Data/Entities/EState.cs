namespace sanchar6tBackEnd.Data.Entities
{
    public class EState
    {
        public char Flag { get; set; }
        public short ID { get; set; }
        public string name { get; set; }
        public byte country_id { get; set; }
        public string? state_code { get; set; }
        public string type { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
        public int CreatedBy { get; set; }

    }
}
