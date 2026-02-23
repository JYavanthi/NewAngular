namespace sanchar6tBackEnd.Data.Entities
{
    public class EAgentDtls
    {
        public char Flag { get; set; }
        public int AgentDtlID { get; set; }
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public long PhoneNumber { get; set; }
        public string CompanyName { get; set; }
        public int CompanyID { get; set; }
        public string CompanyAddress { get; set; }
        public string ShopAddress { get; set; }
        public string? GST { get; set; }
        public string Email { get; set; }
        public string Organisation { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string? Status { get; set; }
        public string Comments { get; set; }
        public int CreatedBy { get; set; }
    }
}
