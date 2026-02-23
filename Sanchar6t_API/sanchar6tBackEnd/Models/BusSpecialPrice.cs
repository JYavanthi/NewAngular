using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class BusSpecialPrice
{
    public int Id { get; set; }

    public int BusBookingDetailId { get; set; }

    public DateTime PriceDate { get; set; }

    public decimal SpecialPrice { get; set; }

    public string? Remark { get; set; }
}
