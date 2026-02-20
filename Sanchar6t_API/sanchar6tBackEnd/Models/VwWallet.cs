using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class VwWallet
{
    public int WalletId { get; set; }

    public int UserId { get; set; }

    public string Amount { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string TransactionLimit { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
