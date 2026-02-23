using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class WeekendConfig
{
    public int Id { get; set; }

    public string DayName { get; set; } = null!;

    public bool? IsWeekend { get; set; }
}
