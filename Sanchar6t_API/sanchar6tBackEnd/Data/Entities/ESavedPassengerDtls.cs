using sanchar6tBackEnd.Models;

namespace sanchar6tBackEnd.Data.Entities
{
    public class ESavedPassengerDtls
    {
      public char Flag { get; set; }
      public int  PassengerDtlID { get; set; }
      public int  UserID         { get; set; }
      public string  FirstName      { get; set; }
      public string?  MiddleName     { get; set; }
      public string  LastName       { get; set; }
      public string Email          { get; set; }
      public string ContactNo      { get; set; }
      public string Gender         { get; set; }
      public string? AadharNo       { get; set; }
      public string? PancardNo      { get; set; }
      public string? BloodGroup     { get; set; }
      public bool  PrimaryUser    { get; set; }
        public DateTime? DOB { get; set; }
        public string?  FoodPref     { get; set; }
      public char CreatedBy { get; set; }
    }
}
