using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class VwCountry
{
    public string Name { get; set; } = null!;

    public string Iso3 { get; set; } = null!;

    public string Iso2 { get; set; } = null!;

    public short NumericCode { get; set; }

    public short Phonecode { get; set; }

    public string? Capital { get; set; }

    public string Currency { get; set; } = null!;

    public string CurrencyName { get; set; } = null!;

    public string CurrencySymbol { get; set; } = null!;

    public string Tld { get; set; } = null!;

    public string? Native { get; set; }

    public string? Region { get; set; }

    public byte RegionId { get; set; }

    public string? Subregion { get; set; }

    public byte SubregionId { get; set; }

    public string Nationality { get; set; } = null!;

    public string Timezones { get; set; } = null!;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string Emoji { get; set; } = null!;

    public string EmojiU { get; set; } = null!;

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
