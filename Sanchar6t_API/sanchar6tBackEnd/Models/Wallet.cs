using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class Wallet
{
    public int WalletId { get; set; }

    public int UserId { get; set; }

    public string Amount { get; set; } = null!;

    public string? Type { get; set; }

    public string? TransactionLimit { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
