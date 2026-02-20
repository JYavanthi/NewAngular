using System;

namespace sanchar6tBackEnd.Data.Entities
{
    public class EAgentInstantRechargeDtl
    {
        public int InstantRechargeId { get; set; }

        public int AgentDtlId { get; set; }
        public int UserId { get; set; }

        public decimal Amount { get; set; }
        public decimal TransactionCharge { get; set; }
        public decimal NetAmount { get; set; }

        public string? PaymentMode { get; set; }
        public string Status { get; set; } = null!;

        public string? ReferenceNo { get; set; }
        public string? Remarks { get; set; }

        public int CreatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }

        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDt { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
