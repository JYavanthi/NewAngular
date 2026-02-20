using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IAgentDtls
    {
        Task<CommonRsult> AgentDtls(EAgentDtls agentDtls);
    }
}
