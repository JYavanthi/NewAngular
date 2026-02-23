using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class BusPriceConfig
{
    public int Id { get; set; }

    public int BusBookingDetailId { get; set; }

    public decimal WeekdayPrice { get; set; }

    public decimal WeekendPrice { get; set; }

    public bool? IsActive { get; set; }
}
