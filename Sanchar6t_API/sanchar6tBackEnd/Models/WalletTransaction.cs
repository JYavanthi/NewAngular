using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class WalletTransaction
{
    public int WalletTrnsnId { get; set; }

    public int UserId { get; set; }

    public string Amount { get; set; } = null!;

    public DateTime Date { get; set; }

    public string Mode { get; set; } = null!;

    public string TransactionNumber { get; set; } = null!;

    public string ErrorCode { get; set; } = null!;

    public string TransactionCode { get; set; } = null!;

    public string Message { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
