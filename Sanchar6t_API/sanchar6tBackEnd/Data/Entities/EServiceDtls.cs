namespace sanchar6tBackEnd.Data.Entities
{
    public class EServiceDtls
    {
        public char Flag { get; set; }
        public int ServiceID { get; set; }
        public int? PackageID { get; set; }
        public string? Servicename { get; set; }
        public string? BusType { get; set; }
        public TimeOnly? Departure { get; set; }  
        public string? Duration { get; set; }
        public TimeOnly? Arrival { get; set; }
        public int? Fare { get; set; }
        public string? WdFrom { get; set; }
        public string? WdTo { get; set; }
        public int? WdFare { get; set; } 
        public string? WeFrom { get; set; }
        public string? WeTo { get; set; }
        public int? WeFare { get; set; }
        public int CreatedBy { get; set; }
    }
}
