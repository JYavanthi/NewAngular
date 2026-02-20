namespace sanchar6tBackEnd.Data.Entities
{
    public class EPackage
    {
        public char Flag { get; set; }
        public int PackageID { get; set; }
        public string PackageName { get; set; }
        public int State { get; set; }
        public int Country { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public int? Noofdays { get; set; }
        public string? Shortdescription { get; set; } 
        public string? Description { get; set; }
        public string? AdditionalNotes { get; set; }
        public int? PackagePrice { get; set; }
        public int CreatedBy { get; set; }
    }
}