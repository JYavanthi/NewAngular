namespace sanchar6tBackEnd.Data.Entities
{
    public class AddAttachment
    {
        public int UserID { get; set; }
        public int PackageID { get; set; }
        public string? Section { get; set; }
        public int Sortorder { get; set; }
        public string AttachmentName { get; set; }
        public int CreatedBy { get; set; }
        public IFormFile file { get; set; }
    }

    public class GetAttachment
    {
        public int PackageID { get; set; }
        //public int attachmentId { get; set; }
        public string Section { get; set; }
        //public int Sortorder { get; set; }
    }
}
