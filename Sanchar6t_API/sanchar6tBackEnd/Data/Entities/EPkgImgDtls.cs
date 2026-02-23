namespace sanchar6tBackEnd.Data.Entities
{
    public class EPkgImageDtls
    {
        public string Flag { get; set; } = string.Empty;
        public int PkgImageID { get; set; }
        public int PackageID { get; set; }
        public string? PkgImage { get; set; }
        public string? PkgSection { get; set; }
        public string? Heading { get; set; }
        public string? SubHeading { get; set; } 
        public string? BtnName { get; set; }
        public string? BtnUrl { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }
}
