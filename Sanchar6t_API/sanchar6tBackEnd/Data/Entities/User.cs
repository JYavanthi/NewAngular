
namespace sanchar6tBackEnd.Data.Entities
{
    public class User
    {
        public string Flag { get; set; }
        public int UserID { get; set; }
        public int UserType { get; set; }
        public string Status { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public string Gender { get; set; }
        public string? AadharNo { get; set; }
        public string? PancardNo { get; set; }
        public string? BloodGroup { get; set; }
        public bool PrimaryUser { get; set; }
        public string? Age { get; set; }
        public string? Address { get; set; }
        public string? AlternativeNumber { get; set; }
        public string? Remarks { get; set; }
        public string CompanyName { get; set; }
        public int CompanyID { get; set; }
        public string CompanyAddress { get; set; }
        public string ShopAddress { get; set; }
        public string Organisation { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Comments { get; set; }
        public string? GST { get; set; }

        public string Amount { get; set; }
        public string? Type { get; set; }
        public string? TransactionLimit { get; set; }
        public int? CreatedBy { get; set; }
    }
}




