using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class BusMasterCode
{
    public int MasterCodeId { get; set; }

    public string Code { get; set; } = null!;

    public string Description { get; set; } = null!;
}
