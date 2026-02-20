namespace sanchar6tBackEnd.Data.Entities
{
    public class EPkgImpNotes
    {
        public char Flag { get; set; }
        public int PkgImpNoteID { get; set; }
        public int PackageID { get; set; } 
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }
}