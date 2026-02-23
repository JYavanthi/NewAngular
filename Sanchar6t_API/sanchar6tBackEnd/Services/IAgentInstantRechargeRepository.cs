using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IAgentInstantRechargeRepository
    {
        CommonRsult Create(EAgentInstantRechargeDtl recharge);
        CommonRsult UpdateStatus(EAgentInstantRechargeDtl recharge);
        CommonRsult GetById(int instantRechargeId);
    }
}
