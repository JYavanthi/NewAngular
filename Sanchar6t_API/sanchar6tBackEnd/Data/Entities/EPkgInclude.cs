namespace sanchar6tBackEnd.Data.Entities
{
    public class EPkgInclude
    {
        public char Flag { get; set; }
        public int PkgIncludeID { get; set; }
        public int PackageID { get; set; }
        public string Description { get; set; } 
        public bool IsIncluded { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }
}
