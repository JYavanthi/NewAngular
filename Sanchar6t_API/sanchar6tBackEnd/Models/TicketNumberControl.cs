using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class TicketNumberControl
{
    public string Prefix { get; set; } = null!;

    public int LastNumber { get; set; }
}
