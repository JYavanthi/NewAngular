using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class Phonepe
{
    public int PhonepeId { get; set; }

    public int? UserId { get; set; }

    public string? MerchantOrderId { get; set; }

    public string? OrderId { get; set; }

    public string? State { get; set; }

    public string? ExpireAt { get; set; }

    public string? RedirectUrl { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
