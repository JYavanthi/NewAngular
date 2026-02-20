namespace sanchar6tBackEnd.Data.Entities
{
    public class EPkgOffer
    {
        public char Flag { get; set; }
        public int PkgOfferID { get; set; }
        public int PackageID { get; set; }
        public int Price { get; set; }
        public bool OfferPrice { get; set; }
        public DateTime EffectiveDate { get; set; }
        public bool WeekDay { get; set; }  
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }
}