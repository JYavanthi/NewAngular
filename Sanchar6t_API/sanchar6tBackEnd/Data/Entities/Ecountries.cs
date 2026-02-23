namespace sanchar6tBackEnd.Data.Entities
{
    public class Ecountries

    {
        public char Flag { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string iso3 { get; set; }
        public string iso2 { get; set; }
        public short numeric_code { get; set; }
        public short phonecode { get; set; }
        public string? capital { get; set; }
        public string currency { get; set; }
        public string currency_name { get; set; }
        public string currency_symbol { get; set; }
        public string tld { get; set; }
        public string? native { get; set; }
        public string? region { get; set; }
        public int region_id { get; set; }
        public string? subregion { get; set; }
        public int subregion_id { get; set; }
        public string nationality { get; set; }
        public string timezones { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string emoji { get; set; }
        public string emojiU { get; set; }
        public int CreatedBy { get; set; }
    }
}
