using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class VwAgentDtl
{
    public int AgentDtlId { get; set; }

    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public string ContactNo { get; set; } = null!;

    public string CompanyName { get; set; } = null!;

    public int CompanyId { get; set; }

    public string CompanyAddress { get; set; } = null!;

    public string ShopAddress { get; set; } = null!;

    public string? Gst { get; set; }

    public string Email { get; set; } = null!;

    public string Organisation { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string? Status { get; set; }

    public string Comments { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
